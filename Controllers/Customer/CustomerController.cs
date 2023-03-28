using IotSupplyStore.DataAccess;
using IotSupplyStore.Models.UpsertModel;
using IotSupplyStore.Models.ViewModel;
using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IotSupplyStore.Controllers.Customer
{
    [Route("api/customer")]
    [Authorize]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }

        [HttpGet]
        [Authorize(Roles = SD.Role_Admin)]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetAllCustomers()
        {
            var model = await _unitOfWork.ApplicationUser.GetAllAsync();
            return Ok(model);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = SD.Role_Admin)]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetCustomer(string id)
        {
            var obj = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(x => x.Id == id,false);
            if (obj == null)
            {
                return NotFound($"ApplicationUsers is null");
            }

            UserVM result = new UserVM()
            {
                FullName = obj.FullName,
                Phone = obj.PhoneNumber,
                Email = obj.Email,
                Avatar = obj.Avatar,
                Address = obj.Address,
                CreatedAt = obj.CreatedAt
            };
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = SD.Role_Customer + "," + SD.Role_Admin)]
        public async Task<IActionResult> UpdateCustomer(string id, UserUpsert customerUpsert)
        {
            var ExistCustomer = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == id);
            if (ExistCustomer == null)
            {
                return NotFound($"ApplicationUsers is null");
            }

            ExistCustomer.FullName = customerUpsert.FullName;
            ExistCustomer.PhoneNumber = customerUpsert.Phone;
            ExistCustomer.Email = customerUpsert.Email;
            ExistCustomer.Avatar = customerUpsert.Avatar;
            ExistCustomer.Address = customerUpsert.Address;
            ExistCustomer.UpdatedAt = DateTime.Now;

            _unitOfWork.ApplicationUser.Update(ExistCustomer);
            await _unitOfWork.Save();
            return Ok("ApplicationUser updated");
        }

        // DELETE: /users/1
        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var user = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"ApplicationUser with id {id} is null");
            }

            _unitOfWork.ApplicationUser.Remove(user);
            await _unitOfWork.Save();
            return Ok("ApplicationUser deleted");
        }
    }
}
