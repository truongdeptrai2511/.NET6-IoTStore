using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using IotSupplyStore.Models.UpsertModel;
using Microsoft.AspNetCore.Authorization;
using IotSupplyStore.Utility;
using System.Net;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Repository.IRepository;

namespace IotSupplyStore.Controllers.Employee
{
    [Route("api/[controller]")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _response;

        public CategoryController(IUnitOfWork db)
        {
            _unitOfWork = db;
            _response = new ApiResponse();
        }

        // GET: api/Category
        [HttpGet]
        [ResponseCache(Duration = 60)]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _unitOfWork.Category.GetAllAsync();

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = categories;
            _response.Message = "success";
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _unitOfWork.Category.GetAllAsync(u => u.Id == id);

            if (category == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Result = null;
                _response.Message = "Not Found";
                _response.ErrorMessages.Add($"Not Found this updateCategory with id {id}");

                return new JsonResult(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = category;
            _response.Message = "Category's got successfully";
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> PutCategory(int id, CategoryUpsert updateCategory)
        {
            var categoryFromDb = await _unitOfWork.Category.GetFirstOrDefaultAsync(u => u.Id == id);
            if (categoryFromDb == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Result = null;
                _response.Message = "Fail to update";
                _response.ErrorMessages.Add($"Category with {id} is not found");

                return new JsonResult(_response);
            }

            categoryFromDb.UpdatedAt = DateTime.Now;
            categoryFromDb.CategoryName = updateCategory.CategoryName;

            try
            {
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(updateCategory.CategoryName))
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Result = null;
                    _response.Message = "Fail to update";
                    _response.ErrorMessages.Add($"Category with {id} is not found");

                    return new JsonResult(_response);
                }
                else
                {
                    throw;
                }
            }

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Result = categoryFromDb;
            _response.Message = "Category's update successfully";
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        // POST: api/Category
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> AddCategory(CategoryUpsert category)
        {
            if (CategoryExists(category.CategoryName))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Result = null;
                _response.Message = "Cannot add this category";
                _response.ErrorMessages.Add("This category has already exist");

                return new JsonResult(_response);
            }
            Category newCategory = new Category()
            {
                CategoryName = category.CategoryName
            };

            await _unitOfWork.Category.Add(newCategory);
            await _unitOfWork.Save();

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Result = newCategory;
            _response.Message = "Category's added successfully";
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Category.GetFirstOrDefaultAsync(u => u.Id == id);
            if (category == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Message = "Not Found";
                _response.Result = null;
                _response.ErrorMessages.Add("Cannot find this category in repository");

                return new JsonResult(_response);
            }

            _unitOfWork.Category.Remove(category);
            await _unitOfWork.Save();

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Message = "Category's deleted";
            _response.Result = null;
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        private bool CategoryExists(string name)
        {
            var entity = _unitOfWork.Category.GetFirstOrDefault(e => e.CategoryName == name);
            return (entity == null) ? false : true;
        }
    }
}
