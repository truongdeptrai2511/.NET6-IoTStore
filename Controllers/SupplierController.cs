using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Models.UpsertModel;
using IotSupplyStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IotSupplyStore.Controllers
{
    [ApiController]
    [Route("api/supplier")]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public SupplierController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var suppliers = await _db.Suppliers.ToListAsync();

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = suppliers;
            _response.Message = "success";
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> GetSupllierById(int id)
        {
            var supplier = await _db.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Result = null;
                _response.Message = "Not Found";
                _response.ErrorMessages.Add($"Not found this supplier with id {id}");

                return new JsonResult(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = supplier;
            _response.Message = "Supllier's got successfully";
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> AddSupplier(SupplierUpsert supplier)
        {
            if (SupplierExists(supplier.SupplierName))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Result = null;
                _response.Message = "Cannot add this supplier";
                _response.ErrorMessages.Add("This supplier has already exist");

                return new JsonResult(_response);
            }
            Suppliers newSupplier = new Suppliers()
            {
                SupplierName = supplier.SupplierName,
                SupplierEmail = supplier.SupplierEmail,
                SupplierPhoneNumber = supplier.SupplierPhoneNumber,
                SupplierFax = supplier.SupplierFax,
            };

            _db.Suppliers.Add(newSupplier);
            await _db.SaveChangesAsync();

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Result = newSupplier;
            _response.Message = "Supplier's added successfully";
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> UpdateSupplier(int id, SupplierUpsert updateSupplier)
        {
            var SupplierFromDb = _db.Suppliers.Find(id);
            if (SupplierFromDb == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Result = null;
                _response.Message = "Fail to update";
                _response.ErrorMessages.Add($"Supplier with {id} is not found");

                return new JsonResult(_response);
            }

            SupplierFromDb.SupplierName = updateSupplier.SupplierName;
            SupplierFromDb.SupplierEmail = updateSupplier.SupplierEmail;
            SupplierFromDb.SupplierPhoneNumber = updateSupplier.SupplierPhoneNumber;
            SupplierFromDb.SupplierFax = updateSupplier.SupplierFax;
            SupplierFromDb.UpdatedAt = DateTime.Now;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (SupplierExists(updateSupplier.SupplierName))
                {
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.Result = null;
                    _response.Message = "Fail to update";
                    //_response.ErrorMessages.Add($"Supplier with {id} is not found");

                    return new JsonResult(_response);
                }
                else
                {
                    throw;
                }
            }

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Result = SupplierFromDb;
            _response.Message = "Supllier's update successfully";
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var SupplierFromDb = await _db.Suppliers.FindAsync(id);
            if (SupplierFromDb == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Message = "Not Found";
                _response.Result = null;
                _response.ErrorMessages.Add("Cannot find this Supplier in repository");

                return new JsonResult(_response);
            }

            _db.Suppliers.Remove(SupplierFromDb);
            await _db.SaveChangesAsync();

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Message = "Supplier's deleted";
            _response.Result = null;
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        private bool SupplierExists(string name)
        {
            return _db.Suppliers.Any(e => e.SupplierName == name);
        }
    }
}
