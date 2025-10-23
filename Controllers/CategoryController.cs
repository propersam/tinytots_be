using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinytots.DbContext;
using Tinytots.DTO;
using Tinytots.Models;

namespace Tinytots.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly TinytotsDbContext _context;

        public CategoryController(TinytotsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                List<Category> categories = await _context.Categories
                    .Include(c => c.SubCategory)
                    .ToListAsync();
                return Ok(categories);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                Category? category = await _context.Categories
                    .Include(c => c.SubCategory)
                    .FirstOrDefaultAsync(c => c.CategoryId == id);

                if (category == null)
                {
                    return NotFound($"Category with Id {id} not found");
                }
                return Ok(category);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Regex regex = new Regex(@"^[a-zA-Z0-9 ]*$");
                bool exists = await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower());
                if (exists)
                {
                    return BadRequest("A Category with the same name already exists.");
                }

                await _context.Categories.AddAsync(category);
                int req = await _context.SaveChangesAsync();
                if (req > 0)
                {
                    return CreatedAtAction(nameof(GetCategoryById),
                        new { id = category.CategoryId },
                        new { Message = "Category created successfully", category }
                    );
                }

                return BadRequest("Category was not created");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Category? existingCategory = await _context.Categories.FindAsync(id);
                if (existingCategory == null)
                {
                    return NotFound($"Category with Id {id} was not found");
                }

                // Check if new name conflicts with existing category
                bool nameExists = await _context.Categories
                    .AnyAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower() && c.CategoryId != id);

                if (nameExists)
                {
                    return Conflict($"A Category with the name '{categoryDto.Name}' already exists");
                }

                existingCategory.Name = categoryDto.Name;
                int req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return Ok(new
                    {
                        Message = "Category updated successfully",
                        Category = existingCategory
                    });
                }

                return BadRequest("Category was not updated");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                Category? category = await _context.Categories
                    .Include(c => c.SubCategory)
                    .FirstOrDefaultAsync(c => c.CategoryId == id);

                if (category == null)
                {
                    return NotFound($"Category with Id {id} was not found");
                }

                // Check if category has associated subcategories
                if (category.SubCategory != null && category.SubCategory.Count > 0)
                {
                    return Conflict($"Cannot delete category '{category.Name}' because it has {category.SubCategory.Count} associated subcategories. Delete the subcategories first.");
                }

                _context.Categories.Remove(category);
                int req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return Ok($"Category '{category.Name}' deleted successfully");
                }

                return BadRequest($"Category with Id {id} was not deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

    }
}











