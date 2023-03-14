using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using Microsoft.AspNetCore.Identity;
using IotSupplyStore.Models.ViewModel;

namespace IotSupplyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() //Get all product database 
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)// Get product by Id
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            Product result = new Product()
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
                UserId = product.UserId,
                P_Code = product.P_Code,
                P_Status = product.P_Status,
                P_Quantity = product.P_Quantity
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)   //Add a new product to the database 
        {
            product.Suppliers = null;
            _context.Products.Add(new Product
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
                UserId = product.UserId,
                P_Code = product.P_Code,
                P_Status = product.P_Status,
                P_Quantity = product.P_Quantity
            });
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)    // Update product
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.CategoryId = product.CategoryId;
            existingProduct.SupplierId = product.SupplierId;
            existingProduct.UserId = product.UserId;
            existingProduct.P_Code = product.P_Code;
            existingProduct.P_Status = product.P_Status;
            existingProduct.P_Quantity = product.P_Quantity;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)      //Delete product
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id) //Check Product Exists
        {
            return _context.Products.Any(e => e.Id == id);
        }

    }

}
