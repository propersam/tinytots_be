using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinytots.DbContext;
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

        [HttpGet("All")]
        public async Task<IActionResult> GetAllSubCategories()
        {
            try
            {
                var req = await _context.SubCategories.ToListAsync();
                if (req.Count > 0)
                {
                    return Ok(req);
                }

                return NotFound("SubCategory is Empty!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
        
      [HttpPost("Create")]
      public async Task<IActionResult> CreateSubCategory( SubCategory subCategoryCreate )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var subcategory = new SubCategory
            {
                Name = subCategoryCreate.Name,
                CategoryId = subCategoryCreate.CategoryId,
                Category =  subCategoryCreate.Category 
            };
            try
            {
                var exists = await _context.SubCategories.AnyAsync(c => c.Name == subCategoryCreate.Name);
               if (exists )
               {
                   return BadRequest("SubCategory with the same name already exists.");
               }
               
               
               await _context.SubCategories.AddAsync(subcategory);
               var req = await _context.SaveChangesAsync();
               if(req > 0)
               {
                  return Ok($"SubCategory..{subcategory.Name} added successfully!");
               } 
              
               return BadRequest($"SubCategory..{subcategory.Name} was not added");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPatch("Update/id/{id}")]
        public async Task<IActionResult> UpdateSubCategory( int id, SubCategory updatedSubCategory )
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
                   return NotFound($"SubCategory with id {id} was not found");
               }
               existingSubCategory.Name = updatedSubCategory.Name;
               var req = await _context.SaveChangesAsync();
               if (req > 0)
               {
                   return Ok($"SubCategory..{updatedSubCategory.Name} updated successfully!");
               }
               
               return BadRequest($"SubCategory..{updatedSubCategory.Name} was not updated");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("Delete/{id}")]
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
                    return Ok($"SubCategory..{existingSubCategory.Name} deleted successfully!");
                }
                return BadRequest($"SubCategory..{existingSubCategory.Name} was not deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        
    }

    
}