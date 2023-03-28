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
        [AllowAnonymous]
        [HttpGet("me")]
        public async Task<IActionResult> TestRole()
        {
            string UserId;
            try
            {
                UserId = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            }
            catch
            {
                return BadRequest("This user hasn't authenticated");
            }

            var ApplicationUser = await _db.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == UserId);
            var UserRoles = await _userManager.GetRolesAsync(ApplicationUser);

            UserVM userVM = new UserVM()
            {
                Id = ApplicationUser.Id,
                FullName = ApplicationUser.FullName,
                Email = ApplicationUser.Email,
                Phone = ApplicationUser.PhoneNumber,
                CreatedAt = ApplicationUser.CreatedAt,
                Role = UserRoles.ToList()
            };
            return Ok(userVM);
        }

        [HttpGet("check-authentication")]
        public IActionResult CheckAuthentication()
        {
            return Ok("authencation success!");
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet("check-is-admin")]
        public IActionResult CheckAdmin()
        {
            return Ok("role admin");
        }
        [Authorize(Roles = SD.Role_Employee)]
        [HttpGet("check-is-employee")]
        public IActionResult CheckEmp()
        {
            return Ok("Role Employee");
        }
        [Authorize(Roles = SD.Role_Customer)]
        [HttpGet("check-is-customer")]
        public IActionResult CheckCustomer()
        {
            return Ok("Role customer");
        }
    }
}
