using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using IotSupplyStore.Models.ViewModel;
using IotSupplyStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IotSupplyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthTestController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthTestController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        private async Task<UserVM> getUserVM(string role)
        {
            var name = User.Claims.First().Value.ToString();
            var model = await _db.User.FirstOrDefaultAsync(u => u.FullName == name);
            UserVM userVM = new UserVM()
            {
                Id = model.Id,
                FullName = name,
                Email = model.Email,
                Phone= model.PhoneNumber,
                Role = role
            };
            return userVM;
        }

        [AllowAnonymous]
        [HttpGet("me")]
        public async Task<IActionResult> TestRole()
        {
            var isAdmin = User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == SD.Role_Admin);
            if (isAdmin)
            {
                var result = await getUserVM(SD.Role_Admin);
                return Ok(result);
            }
            var isEmployee = User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == SD.Role_Employee);
            if (isEmployee)
            {
                var result = await getUserVM(SD.Role_Employee);
                return Ok(result);
            }
            else
            {
                var result = await getUserVM(SD.Role_Customer);
                return Ok(result);
            }
        }


        [HttpGet("CheckAuthentication")]
        public IActionResult CheckAuthentication()
        {
            return Ok("authencation success!");
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet("CheckIsAdmin")]
        public IActionResult CheckAdmin()
        {
            return Ok("role admin");
        }
        [Authorize(Roles = SD.Role_Employee)]
        [HttpPost("CheckIsEmployee")]
        public IActionResult CheckEmp()
        {
            return Ok("Role Employee");
        }
        [Authorize(Roles = SD.Role_Customer)]
        [HttpPut("CheckIsCustomer")]
        public IActionResult CheckCustomer()
        {
            return Ok("Role customer");
        }
    }
}
