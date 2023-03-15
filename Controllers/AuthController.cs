using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IotSupplyStore.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using IotSupplyStore.DataAccess;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace IotSupplyStore.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthController(ApplicationDbContext db, IConfiguration options,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            secretKey = options.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _roleManager = roleManager;
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
                IdentityResult result;
                try
                {
                    result = await _userManager.CreateAsync(newAdmin, model.Password);
                }
                catch (Exception)
                {
                    return BadRequest("Problem occurs when assign username or full name! Please re-check again!");
                }
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
            ApplicationUser userFromDb = _db.User.FirstOrDefault(u => u.FullName.ToLower() == model.Name.ToLower());
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
                IdentityResult result;
                try
                {
                    result = await _userManager.CreateAsync(newUser, model.Password);
                }
                catch (Exception)
                {
                    return BadRequest("Problem occurs when assign username or full name! Please re-check again!");
                }
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
        public async Task<IActionResult> RegisterEmployee(EmployeeRequest empRequest)
        {
            ApplicationUser newEmployee = new()
            {
                UserName = empRequest.UserName,
                Email = empRequest.Email,
                NormalizedEmail = empRequest.Email.ToUpper(),
                FullName = empRequest.Name,
                PhoneNumber = empRequest.PhoneNumber,
                Address = empRequest.Address,
                citizenIdentification = empRequest.citizenIdentification
            };
            string FirstPassword = "abcde12345";
            try
            {
                IdentityResult result;
                try
                {
                    result = await _userManager.CreateAsync(newEmployee, FirstPassword);
                }
                catch (Exception)
                {
                    return BadRequest("Problem occurs when assign username or full name! Please re-check again!");
                }
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Role_Employee).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
                    }
                    await _userManager.AddToRoleAsync(newEmployee, SD.Role_Employee);
                    return Ok("Employee registered");
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
                return BadRequest(new LoginResponseDTO());
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
    }
}
