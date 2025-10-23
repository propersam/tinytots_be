using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinytots.DbContext;
using Tinytots.DTO;
using Tinytots.Models;

namespace Tinytots.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoryController : ControllerBase
    {
        private readonly TinytotsDbContext _context;
        
       public SubCategoryController(TinytotsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubCategories()
        {
            try
            {
                var subCategories = await _context.SubCategories
                    .Include(sc => sc.Category)
                    .ToListAsync();

                return Ok(subCategories);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubCategoryById(int id)
        {
            try
            {
                var subCategory = await _context.SubCategories
                    .Include(sc => sc.Category)
                    .FirstOrDefaultAsync(sc => sc.SubCategoryId == id);

                if (subCategory == null)
                {
                    return NotFound($"SubCategory with Id {id} was not found");
                }

                return Ok(subCategory);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

      [HttpPost]
      public async Task<IActionResult> CreateSubCategory([FromBody] SubCategoryDTO subCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if category exists
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == subCategoryDto.CategoryName.ToLower());

                if (category == null)
                {
                    return NotFound($"Category '{subCategoryDto.CategoryName}' does not exist");
                }

                // Check for duplicate subcategory name
                var exists = await _context.SubCategories
                    .AnyAsync(sc => sc.Name.ToLower() == subCategoryDto.Name.ToLower());

                if (exists)
                {
                    return Conflict("SubCategory with the same name already exists.");
                }

                var subcategory = new SubCategory
                {
                    Name = subCategoryDto.Name,
                    CategoryId = category.CategoryId
                };

                await _context.SubCategories.AddAsync(subcategory);
                var req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return CreatedAtAction(nameof(GetSubCategoryById),
                        new { id = subcategory.SubCategoryId },
                        new
                        {
                            Message = $"SubCategory '{subcategory.Name}' added successfully!",
                            SubCategory = subcategory
                        });
                }

                return BadRequest($"SubCategory '{subcategory.Name}' was not added");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateSubCategory(int id, SubCategoryDTO subCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var existingSubCategory = await _context.SubCategories.FindAsync(id);
                if (existingSubCategory == null)
                {
                    return NotFound($"SubCategory with Id {id} was not found");
                }

                existingSubCategory.Name = subCategoryDto.Name;
                var req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return Ok(new
                    {
                        Message = "SubCategory updated successfully",
                        SubCategory = existingSubCategory
                    });
                }

                return BadRequest("SubCategory was not updated");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            try
            {
                
                var existingSubCategory = await _context.SubCategories.FindAsync(id);
                if (existingSubCategory == null)
                {
                    return NotFound($"SubCategory with id {id} was not found");
                }
                _context.SubCategories.Remove(existingSubCategory);
                var req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return Ok($"SubCategory '{existingSubCategory.Name}' deleted successfully");
                }

                return BadRequest($"SubCategory with Id {id} was not deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
        
        
    }

    
}