using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using IotSupplyStore.Models.UpsertModel;
using Microsoft.AspNetCore.Authorization;
using IotSupplyStore.Utility;

namespace IotSupplyStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/Category
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _db.Categories.ToListAsync();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _db.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            category.UpdatedAt = DateTime.Now;
            _db.Entry(category).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.C_Name))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Updated");
        }

        // POST: api/Category
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<ActionResult<Category>> PostCategory(CategoryUpsert category)
        {
            if (!CategoryExists(category.C_Name))
            {
                return BadRequest("This category has already exist");
            }
            Category newCategory = new Category()
            {
                C_Name = category.C_Name,
                C_Home = category.C_Home,
                C_Icon = category.C_Icon
            };

            _db.Categories.Add(newCategory);
            await _db.SaveChangesAsync();
            return Ok("Category created");
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(string name)
        {
            return _db.Categories.Any(e => e.C_Name == name);
        }
    }
}
