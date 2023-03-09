using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;

namespace IotSupplyStore.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<Product>> GetProducts()
        {
            var products = ProductFakeApi.GenerateProducts(10);
            return Ok(products);
        }
        /*        [HttpGet]
                public async Task<ActionResult<IEnumerable<Product>>> GetProducts() //Get all product database 
                {
                    return await _context.Products.ToListAsync();
                }*/

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)// Get product by Id
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)   //Add a new product to the database 
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)    // Update product
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

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
