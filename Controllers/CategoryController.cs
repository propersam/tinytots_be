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

        [HttpGet("All")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var req = await _context.Categories.ToListAsync();
                if (req.Count > 0)
                {
                    return Ok(req);
                }
                return Ok("[]");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound("Category not found");
                }
                return Ok(category);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
            
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Regex regex = new Regex(@"^[a-zA-Z0-9 ]*$");
                var exists = await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower());
                if (exists)
                {
                    return BadRequest("A Category with the same name already exists.");
                }
                
                await _context.Categories.AddAsync(category);
                var req = await _context.SaveChangesAsync();
                if (req > 0)
                {
                    return CreatedAtAction(nameof(GetCategoryByName),
                        new { name = category.Name },
                        new { Message = $"Category was Successfully Created!", category }
                    );
                }

                return BadRequest("A new Category was not created.");
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");

            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            try
            {
                var product = await _context.Categories
                    .Where(x => x.Name == name)
                    .ToListAsync();
                if (product.Count > 0)
                {
                    return Ok(product);
                }

                return NotFound($"The Category with the name..'{name}'..could not be Found.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    } 
}


    
    
    
    
    
    
    
    
    
