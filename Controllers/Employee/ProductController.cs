using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using IotSupplyStore.Models.DtoModel;
using System.Net;
using IotSupplyStore.Models.UpsertModel;
using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.Utility;

namespace IotSupplyStore.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = SD.Policy_ProductManager)]
    public class ProductController : ControllerBase
    {
        private ApiResponse _response;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
        }

        [HttpGet]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult> GetAllProducts() //Get all newProduct database 
        {
            var ProductList = await _unitOfWork.Product.GetAllAsync();

            _response.StatusCode = HttpStatusCode.OK;
            _response.Message = "All Products're got";
            _response.Result = ProductList;
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<Product>> GetProduct(int id)// Get newProduct by Id
        {
            var product = await _unitOfWork.Product.GetFirstOrDefaultAsync(x => x.Id == id,false);

            if (product == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Message = "Not Found";
                _response.Result = null;
                _response.ErrorMessages.Add("Cannot find any products in repository");

                return new JsonResult(_response);
            }
            Product result = new Product()
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
                Code = product.Code,
                Status = product.Status,
                Quantity = product.Quantity,
                ImgName = product.ImgName,
                Description = product.Description,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
            _response.StatusCode = HttpStatusCode.OK;
            _response.Message = "Product's got";
            _response.Result = result;
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        // This method is decorated with the HttpPost attribute,
        // which indicates that it is a POST endpoint that can be accessed via HTTP
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(ProductUpsert newProduct)
        {
            // The method receives a ProductUpsert object in the request body,
            // which is used to create a new Product entity that is added to the database

            // Add the new product to the database context
            await _unitOfWork.Product.Add(new Product
            {
                ProductName = newProduct.ProductName,
                CategoryId = newProduct.CategoryId,
                SupplierId = newProduct.SupplierId,
                Code = newProduct.Code,
                Status = newProduct.Status,
                Quantity = newProduct.Quantity,
                Price = newProduct.Price,
                Description = newProduct.Description,
                ImgName = newProduct.ImgName
            });

            // Save changes to the database
            await _unitOfWork.Save();

            // Create an HTTP response indicating that the operation was successful
            _response.StatusCode = HttpStatusCode.OK;
            _response.Message = "Add product success";
            _response.Result = newProduct;
            _response.ErrorMessages = null;

            // Return the response as a JsonResult
            return new JsonResult(_response);
        }

        [HttpPut("{id}")]
        // Update newProduct
        public async Task<IActionResult> UpdateProduct(int id, ProductUpsert updateProduct)
        {
            var existingProduct = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == id);
            if (existingProduct == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Message = "Not Found";
                _response.Result = null;
                _response.ErrorMessages.Add("Cannot find any products in repository");
            }

            existingProduct.CategoryId = updateProduct.CategoryId;
            existingProduct.SupplierId = updateProduct.SupplierId;
            existingProduct.Code = updateProduct.Code;
            existingProduct.Status = updateProduct.Status;
            existingProduct.Quantity = updateProduct.Quantity;
            existingProduct.UpdatedAt = DateTime.Now;
            existingProduct.Price = updateProduct.Price;
            existingProduct.Description = updateProduct.Description;
            existingProduct.ImgName = updateProduct.ImgName;

            _unitOfWork.Product.Update(existingProduct);
            await _unitOfWork.Save();

            _response.StatusCode = HttpStatusCode.OK;
            _response.Message = "Product's updated";
            _response.Result = existingProduct;
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)      //Delete newProduct
        {
            var product = await _unitOfWork.Product.GetFirstOrDefaultAsync(u => u.Id == id);
            if (product == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Message = "Not Found";
                _response.Result = null;
                _response.ErrorMessages.Add("Cannot find any products in repository");
            }

            _unitOfWork.Product.Remove(product);
            await _unitOfWork.Save();

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.Message = "Product's deleted";
            _response.Result = null;
            _response.ErrorMessages = null;

            return new JsonResult(_response);
        }
    }
}
