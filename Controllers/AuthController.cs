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

namespace IotSupplyStore.Controllers
{
    [Route("api/auth")]
    [ApiController]
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
        [HttpPost("register/customer")] //For Customer
        public async Task<IActionResult> Register([FromBody] CustomerRegisterRequestDTO model)
        {
            ApplicationUser userFromDb = _db.User.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
            if (userFromDb != null)
            {
                return BadRequest("This user has already exist!!!");
            }

            ApplicationUser newUser = new()
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
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
                    }
                    //if (model.Role.ToLower() == SD.Role_Admin)
                    //{
                    //    await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                    //}
                    //else if (model.Role.ToLower() == SD.Role_Employee)
                    //{
                    //    await _userManager.AddToRoleAsync(newUser, SD.Role_Employee);
                    //}
                    //else if (model.Role.ToLower() == SD.Role_Customer)
                    //{
                    //    await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                    //}
                    //else
                    //{
                    //    return BadRequest();
                    //}
                    await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                    return Ok("Registered");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return BadRequest();
        }
        [HttpPost("register/employee")] //For Employee Request
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeRegisterRequestDTO model)
        {
            ApplicationUser userFromDb = _db.User.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
            if (userFromDb != null)
            {
                return BadRequest("This user has already exist!!!");
            }
            if (model.citizenIdentification == null)
            {
                return BadRequest("This employee must have Citizen Identification");
            }

            ApplicationUser newUser = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                FullName = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                citizenIdentification = model.citizenIdentification,
            };
            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
                    }
                    await _userManager.AddToRoleAsync(newUser, SD.Role_Employee);
                    return Ok("Registered");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return BadRequest();
        }
        [HttpPost("login")]
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
