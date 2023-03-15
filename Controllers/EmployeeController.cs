using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IotSupplyStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetEmployee()
        {
            var role = await _roleManager.FindByNameAsync(SD.Role_Employee);
            if (role == null)
            {
                return NotFound();
            }
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            return Ok(usersInRole);
        }
        [HttpGet("id")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<ActionResult<ApplicationUser>> GetEmployeeById(string id)
        {
            var role = await _roleManager.FindByNameAsync(SD.Role_Employee);
            if (role == null)
            {
                return NotFound();
            }
            var userInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            var employee = userInRole.FirstOrDefault(u => u.Id == id);
            return Ok(employee);
        }

        [HttpPost("request")] //For Employee Request to Become a Employee of Store
        [AllowAnonymous]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeRegisterRequestDTO model)
        {
            ApplicationUser userFromDb = await _db.User.FirstOrDefaultAsync(u => u.FullName.ToLower() == model.Name.ToLower());
            if (userFromDb != null)
            {
                return BadRequest("This user has already exist!!!");
            }
            if (model.citizenIdentification == null)
            {
                return BadRequest("This employee must have Citizen Identification");
            }

            var newEmployeeRequest = new EmployeeRequest()
            {
                UserName = model.UserName,
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                AvatarLink = model.AvatarLink,
                Address = model.Address,
                citizenIdentification = model.citizenIdentification
            };
            _db.EmployeeRequests.Add(newEmployeeRequest);
            _db.SaveChanges();

            return Ok("Requested");
        }
    }
}
