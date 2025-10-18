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
        
        [HttpGet("All")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var allProducts = await _context.Products
                    .Select(p => new ProductDTO
                    {
                        Name = p.Name,
                        Price = p.UnitPrice,
                        Quantity = p.Quantity
                        
                    } )
                    .ToListAsync();
                return Ok(allProducts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
               var product = await _context.Products.FindAsync(id);
               
               if (product == null)
               {
                   return NotFound($"Product with Id {id} was not found");
               }
               return Ok(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct(ProductDTO addProduct, CategoryDTO categoryName )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check for subcategory
                var subCategory = await _context.SubCategories
                    .FirstOrDefaultAsync(sc => sc.Name.ToLower() == addProduct.SubCategoryName.ToLower());

                if (subCategory == null)
                {
                    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.Name.ToLower());
                    
                    if (category == null)
                    {
                        return NotFound($"Category '{categoryName.Name}' does not exist.");
                    }

                    subCategory = new SubCategory 
                    { 
                        Name = addProduct.SubCategoryName,
                        Category = category
                    };
                    await _context.SubCategories.AddAsync(subCategory);
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
                                 new { Message = $"The New Product..'{product.Name}'..has been added successfully!", product}
                       );
                }
                return BadRequest($"The product '{product.Name}' was not added..");
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpGet("Name/{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            try
            {
                var product = await _context.Products
                    .Where(x => x.Name == name)
                    .ToListAsync();

                if (product.Count == 0)
                {
                    return NotFound($"No products found with the Name '{name}'.");
                }
                return Ok(product);
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpGet("SubCategory/{subCategory}")]
        public async Task<IActionResult> GetProductWithSubCategory(string subCategory)
        {
            try
            {
                var product = await _context.Products
                    .Where(x => x.SubCategory.Name == subCategory)
                    .ToListAsync();
                if (product.Count == 0)
                {
                    return NotFound($"Cannot filter with this subcategory '{subCategory}'");
                }
                return Ok(product);
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateProduct(ProductDTO updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == updatedProduct.Name.ToLower());
                if (existingProduct == null)
                {
                    return NotFound($"No Product with ID of {updatedProduct.Name} was found..");
                }

                var subCategory = await _context.SubCategories
                    .FirstOrDefaultAsync(sc => sc.Name.ToLower() == updatedProduct.SubCategoryName.ToLower());
                
                if (subCategory == null)
                {
                    return NotFound("SubCategory does not exist.");
                }
                
                existingProduct.Name = updatedProduct.Name;
                existingProduct.SubCategory = subCategory;
                existingProduct.UnitPrice = updatedProduct.Price;
                existingProduct.Quantity = updatedProduct.Quantity;
                
                var req = await _context.SaveChangesAsync();
                if (req > 0)
                {
                    return Ok($"The Product named {existingProduct.Name} has been updated successfully");
                }
                return BadRequest($"No data in the product '{existingProduct.Name}' was changed..");
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpDelete("Delete/id/{id}")]
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