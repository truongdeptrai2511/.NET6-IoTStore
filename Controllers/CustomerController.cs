using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using IotSupplyStore.Models.UpsertModel;
using IotSupplyStore.Models.ViewModel;
using IotSupplyStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IotSupplyStore.Controllers
{
    [Route("api/customer")]
    [Authorize]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public CustomerController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> GetAllCustomers()
        {
            var model = await _db.User.ToListAsync();
            return Ok(model);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = SD.Role_Customer + "," + SD.Role_Admin)]
        public async Task<IActionResult> GetCustomer(string id)
        {
            var obj = await _db.User.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
            {
                return NotFound($"User is null");
            }

            UserVM result = new UserVM()
            {
                FullName = obj.FullName,
                Phone = obj.PhoneNumber,
                Email = obj.Email,
                Avatar = obj.Avatar,
                Address = obj.Address,
                CreatedAt = obj.CreatedAt,
                UpdatedAt = obj.UpdatedAt,
            };
            return Ok(result);
        }

        [Authorize(Roles = SD.Role_Customer + "," + SD.Role_Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, UserUpsert customerUpsert)
        {
            var ExistCustomer = await _db.User.FirstOrDefaultAsync(u => u.Id == id);
            if (ExistCustomer == null)
            {
                return NotFound($"User is null");
            }

            ExistCustomer.FullName = customerUpsert.FullName;
            ExistCustomer.PhoneNumber = customerUpsert.Phone;
            ExistCustomer.Email = customerUpsert.Email;
            ExistCustomer.Avatar = customerUpsert.Avatar;
            ExistCustomer.Address = customerUpsert.Address;
            ExistCustomer.UpdatedAt = DateTime.Now;

            _db.Entry(ExistCustomer).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Ok("ApplicationUser updated");
        }

        // DELETE: /users/1
        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var user = await _db.User.FindAsync(id);

            if (user == null)
            {
                return NotFound($"ApplicationUser with id {id} is null");
            }

            _db.User.Remove(user);
            await _db.SaveChangesAsync();
            return Ok("ApplicationUser deleted");
        }
    }
}
