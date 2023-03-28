using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using IotSupplyStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IotSupplyStore.Controllers.Employee
{
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Shipper)]
    [ApiController]
    [Route("api/shipper")]
    public class ShipperController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShipperController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> GetAllShipper() 
        {
            var ShipperList = await _userManager.GetUsersInRoleAsync(SD.Role_Shipper);
            if (ShipperList == null)
            {
                return NotFound("No shipper found");
            }
            return Ok(ShipperList);
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet("id")]
        public async Task<IActionResult> GetShipperById (string id)
        {
            var Shipper = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (Shipper == null)
            {
                return NotFound($"this shipper with id {id} not found");
            }
            return Ok(Shipper);
        }

        
    }
}
