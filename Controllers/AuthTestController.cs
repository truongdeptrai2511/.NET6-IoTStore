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
        [HttpGet]
        public IActionResult CheckAuthentication()
        {
            return Ok("authencation success!");
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet("{id:int}")]
        public IActionResult CheckAdmin(int id = 1)
        {
            return Ok("role admin");
        }
        [Authorize(Roles = SD.Role_Employee)]
        [HttpPost]
        public IActionResult CheckEmp(int id)
        {
            return Ok("role emp");
        }
        [Authorize(Roles = SD.Role_Customer)]
        [HttpPut]
        public IActionResult CheckCustomer()
        {
            return Ok("Role customer");
        }
    }
}
