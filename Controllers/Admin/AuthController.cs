using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using IotSupplyStore.Utility;
using IotSupplyStore.DataAccess;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Models;
using IotSupplyStore.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace IotSupplyStore.Controllers.Admin
{
    [Route("api/auth")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private string secretKey;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        public AuthController(ApplicationDbContext db, IConfiguration options,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IEmailService emailService)
        {
            _db = db;
            secretKey = options.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin(AdminRegisterRequestDTO model)
        {
            ApplicationUser newAdmin = new()
            {
                UserName = model.UserName,
                FullName = model.FullName
            };
            try
            {
                IdentityResult result = await _userManager.CreateAsync(newAdmin, model.Password);

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                    }
                    await _userManager.AddToRoleAsync(newAdmin, SD.Role_Admin);
                    return Ok("Admin registered");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return BadRequest("please re-check the information");
        }

        [AllowAnonymous]
        [HttpPost("register/customer")] //For Customer
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegisterRequestDTO model)
        {
            ApplicationUser userFromDb = _db.User.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
            if (userFromDb != null)
            {
                return BadRequest("This user has already exist!!!");
            }

            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                FullName = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };
            try
            {
                IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    }
                    await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                    return Ok("Registered");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return BadRequest("Failed to register new user!");
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost("register/employee")]
        public async Task<IActionResult> RegisterEmployee(int EmployeeRequestId)
        {
            var empRequest = await _db.EmployeeRequests.FirstOrDefaultAsync(u => u.Id == EmployeeRequestId);
            if (empRequest == null)
            {
                return NotFound($"Cannot find this employee with id {EmployeeRequestId}");
            }

            ApplicationUser newEmployee = new()
            {
                UserName = empRequest.UserName,
                Email = empRequest.Email,
                NormalizedEmail = empRequest.Email.ToUpper(),
                FullName = empRequest.Name,
                PhoneNumber = empRequest.PhoneNumber,
                Address = empRequest.Address,
                citizenIdentification = empRequest.CitizenIdentification
            };
            string FirstPassword = Guid.NewGuid().ToString();
            try
            {
                IdentityResult result = await _userManager.CreateAsync(newEmployee, FirstPassword);
                if (result.Succeeded)
                {
                    if (empRequest.Role == SD.Role_Employee)
                    {
                        if (!_roleManager.RoleExistsAsync(SD.Role_Employee).GetAwaiter().GetResult())
                        {
                            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
                            await _userManager.AddToRoleAsync(newEmployee, SD.Role_Employee);
                        }
                    }
                    else if (empRequest.Role == SD.Role_Shipper)
                    {
                        if (!_roleManager.RoleExistsAsync(SD.Role_Shipper).GetAwaiter().GetResult())
                        {
                            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Shipper));
                            await _userManager.AddToRoleAsync(newEmployee, SD.Role_Shipper);
                        }
                    }
                    else
                    {
                        return BadRequest("this role isn't support at the moment");
                    }
                    _db.EmployeeRequests.Remove(empRequest);
                    return Ok("registered");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return BadRequest("please re-check the information");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            ApplicationUser userFromDb = _db.User.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);
            if (isValid == false)
            {
                return BadRequest("username or password is incorrect");
            }
            // if login success, have to generate JWT Token
            var roles = await _userManager.GetRolesAsync(userFromDb);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("fullName", userFromDb.FullName),
                    new Claim("id", userFromDb.Id.ToString()),
                    new Claim(ClaimTypes.Email, userFromDb.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponse = new()
            {
                UserName = userFromDb.UserName,
                Token = tokenHandler.WriteToken(token),
            };
            if (loginResponse.UserName == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                ModelState.AddModelError("error login", "Username or password is incorrect");
                return BadRequest(ModelState);
            }
            return Ok(loginResponse);
        }

        #region password process

        [AllowAnonymous]
        [HttpPost("forgot-password-request")]
        public async Task<IActionResult> ForgotPasswordRequest(ForgotPassword model)
        {
            if (IsValidEmail(model.Email))
            {
                var user = await _db.User.FirstOrDefaultAsync(x => x.Email == model.Email);
                if (user == null)
                {
                    return BadRequest("this email has not been already exist");
                }

                EmailDto emailRequest = new EmailDto()
                {
                    ToName = user.FullName,
                    ToEmailAddress = model.Email,
                    Subject = "Please reset password",
                    Body = $"Hi {user.FullName},\r\nWe received a request to reset your Thuphigiaothong.com password.\r\nPlease click this Link: {model.Link} to reset your password\r\nAlternatively, you can directly change your password."
                };

                await _emailService.SendMail(emailRequest);
                return Ok("sent");
            }
            return BadRequest($"{model.Email} is an invalid Email address");
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            var applicationUser = await _db.User.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());
            //var model = User.FindFirstValue("fullname").ToString(); TODO
            if (applicationUser == null)
            {
                return BadRequest("cant find this user");
            }

            var result = await _userManager.ChangePasswordAsync(applicationUser, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok("password changed");
            }

            return BadRequest("password changes fail");
        }

        #endregion

        #region process function
        bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            try
            {
                // Use the built-in MailAddress class to validate the Email format
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
