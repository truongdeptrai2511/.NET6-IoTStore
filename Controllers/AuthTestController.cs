using IotSupplyStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IotSupplyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthTestController : ControllerBase
    {
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
