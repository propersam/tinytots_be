using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinytots.DbContext;
using Tinytots.DTO;
using Tinytots.Enums;
using Tinytots.Models;
using Tinytots.Controllers;


namespace Tinytots.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly TinytotsDbContext _context;
       public ProductController(TinytotsDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.SubCategory)
                        .ThenInclude(sc => sc.Category)
                    .ToListAsync();
                return Ok(products);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.SubCategory)
                        .ThenInclude(sc => sc.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                {
                    return NotFound($"Product with Id {id} was not found");
                }
                return Ok(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDTO addProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check for subcategory - must exist before creating product
                var subCategory = await _context.SubCategories
                    .FirstOrDefaultAsync(sc => sc.Name.ToLower() == addProduct.SubCategoryName.ToLower());

                if (subCategory == null)
                {
                    return NotFound($"SubCategory '{addProduct.SubCategoryName}' does not exist. Please create the subcategory first.");
                }

                // Prevent duplicate product names
                var exists = await _context.Products.AnyAsync(p => p.Name.ToLower() == addProduct.Name.ToLower());
                if (exists)
                {
                    return Conflict($"A Product with the Name '{addProduct.Name}' already exists");
                }

                // Create product
                var product = new Product
                {
                    Name = addProduct.Name,
                    SubCategoryId = subCategory.SubCategoryId,
                    Quantity = addProduct.Quantity,
                    UnitPrice = addProduct.Price
                };

                await _context.Products.AddAsync(product);
                var req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return CreatedAtAction(nameof(GetProductById),
                        new { id = product.ProductId },
                        new { Message = "Product created successfully", product }
                    );
                }
                return BadRequest("Product was not created");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with Id {id} was not found");
                }

                var subCategory = await _context.SubCategories
                    .FirstOrDefaultAsync(sc => sc.Name.ToLower() == updatedProduct.SubCategoryName.ToLower());

                if (subCategory == null)
                {
                    return NotFound($"SubCategory '{updatedProduct.SubCategoryName}' does not exist");
                }

                existingProduct.Name = updatedProduct.Name;
                existingProduct.SubCategoryId = subCategory.SubCategoryId;
                existingProduct.UnitPrice = updatedProduct.Price;
                existingProduct.Quantity = updatedProduct.Quantity;

                var req = await _context.SaveChangesAsync();
                if (req > 0)
                {
                    return Ok(new
                    {
                        Message = "Product updated successfully",
                        Product = existingProduct
                    });
                }
                return BadRequest("Product was not updated");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                
                _context.Products.Remove(product);
               var req = await _context.SaveChangesAsync() > 0;
               if (req)
               {
                   return Ok($"Product with id {id} deleted successfully");
               }
               return StatusCode(400, $"Product with id {id} was not deleted");
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
    }
}