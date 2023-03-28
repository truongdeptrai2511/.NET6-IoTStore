using IotSupplyStore.Models;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Models.UpsertModel;
using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IotSupplyStore.Controllers.Employee
{
    [ApiController]
    [Route("api/supplier")]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;

        public SupplierController(IUnitOfWork db)
        {
            _unitOfWork = db;
            _response = new ApiResponse();
        }

        [ResponseCache(Duration = 60)]
        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var suppliers = await _unitOfWork.Supplier.GetAllAsync();

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = suppliers;
            _response.Message = "success";
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        [ResponseCache(Duration = 60)]
        [HttpGet("{id}")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> GetSupllierById(int id)
        {
            var supplier = await _unitOfWork.Supplier.GetFirstOrDefaultAsync(x => x.Id == id, false);

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
            var CheckNameDuplication = await _unitOfWork.Supplier.GetFirstOrDefaultAsync(u => u.SupplierName == supplier.SupplierName);
            if (CheckNameDuplication != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Result = null;
                _response.Message = "Cannot assign this supplier, may be the name is duplicated";
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

            await _unitOfWork.Supplier.Add(newSupplier);
            await _unitOfWork.Save();

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
            var SupplierFromDb = await _unitOfWork.Supplier.GetFirstOrDefaultAsync(x => x.Id == id, false);
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

            await _unitOfWork.Save();

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
            var SupplierFromDb = await _unitOfWork.Supplier.GetFirstOrDefaultAsync(x => x.Id == id, false);
            if (SupplierFromDb == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Message = "Not Found";
                _response.Result = null;
                _response.ErrorMessages.Add("Cannot find this Supplier in repository");

                return new JsonResult(_response);
            }

            _unitOfWork.Supplier.Remove(SupplierFromDb);
            await _unitOfWork.Save();

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Message = "Supplier's deleted";
            _response.Result = null;
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }
    }
}
