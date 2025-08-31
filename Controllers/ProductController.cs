using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinytots.DbContext;
using Tinytots.DTO;
using Tinytots.Enums;
using Tinytots.Interfaces;
using Tinytots.Models;


namespace Tinytots.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase 
{
    private readonly TinytotsDbContext _productcontext;
    public ProductController(TinytotsDbContext ProductContext)
    {
        _productcontext = ProductContext;
    }
        
        
    [HttpGet("ShowAllProducts")]
    public async Task <IActionResult> GetAllProducts()
    {
        try
        {
         var allProducts = await _productcontext.Products.ToListAsync(); 
         return Ok(allProducts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpPost("AddNewProduct")]
    public async Task <IActionResult> CreateProduct([FromBody] ProductDTO addProduct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var product = new Product
            {
                Name = addProduct.Name,
                Gender = addProduct.Gender,
                AgeGroup = addProduct.AgeGroup,
                CategoryId = addProduct.CategoryId,
                SubCategoryId = addProduct.SubCategoryId,
                OutOfStock = addProduct.OutOfStock, 
                Price = addProduct.Price,
            };   
                
              var adding= _productcontext.Products.AddAsync(product);
               await _productcontext.SaveChangesAsync();
               return Ok(new { Message = "Product added successfully", ProductId = product.Id });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpGet("Product/{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        try
        {
            var product = await _productcontext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product != null)
            {
                return Ok(product);
            }

            return NotFound(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpGet("Product/{name}")]
    public async Task<IActionResult> GetProductByName(string name)
    {
        try
        {
            var product = await _productcontext.Products
                .Where(x => x.Name == name)
                .ToListAsync();
            
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound(name);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpGet("FilterProductWith/{Age}")]
    public async Task<IActionResult> GetProductByAge(AgeGroupEnum age)
    {
        try
        {
            var product = _productcontext.Products
                .Where(x => x.AgeGroup == age)
                .ToListAsync<Product>();

            if (product != null)
            {
                return Ok(product);
            }
            return NotFound(age);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
        
        [HttpGet("FilterProductsWith/{gender}")]
        async Task<IActionResult> GetProductByGender(GenderEnum gender)
        {
            try
            {
                var product = _productcontext.Products
                    .Where(x => x.Gender == gender)
                    .ToListAsync<Product>();
                if (product != null)
                {

                    return Ok(product);
                }
                return NotFound(gender);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
    }
    
    [HttpGet("FilterProductsWith/{Category}")]
    public async Task<IActionResult> GetProductByCategory(string category)
    {
        try
        {
            var product = _productcontext.Products
                .Where(x => x.Category.CatName == category)
                .ToListAsync<Product>();
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound(category);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpGet("FilterProductsWith/{subCategory}")]
    public async Task<IActionResult> GetProductWithSubCAtegory(string subCategory)
    {
        try
        {
            var product = _productcontext.Products
                .Where(x => x.SubCategory.Name == subCategory)
                .ToListAsync<Product>();
            if (product != null)
            {
                return Ok(product);
            }
            return StatusCode(404, $"Can't filter with {subCategory}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
      
    }        
    
    [HttpPatch("UpdateProduct/{id}")]
    public async Task<IActionResult> UpdateProduct(ProductDTO updatedProduct, int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        { 
            var existingProduct = await _productcontext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (existingProduct == null)
            {
                return NotFound($"Product with ID {id} not found");
            }

            existingProduct.Name = updatedProduct.Name;
           existingProduct.Gender = updatedProduct.Gender;
           existingProduct.AgeGroup = updatedProduct.AgeGroup;
           existingProduct.CategoryId = updatedProduct.CategoryId;
           existingProduct.SubCategoryId = updatedProduct.SubCategoryId;
           existingProduct.Price = updatedProduct.Price;
           existingProduct.OutOfStock = updatedProduct.OutOfStock;
           var req = await _productcontext.SaveChangesAsync();
           if (req > 0)
           {
               return Ok($"Product with name {existingProduct.Name} updated successfully");
           }
           {
               return StatusCode(400, $"Product with name {existingProduct.Name} was not updated");
           }
           
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpPost("DeleteProduct/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _productcontext.Products.FirstOrDefaultAsync(x => x.Id == id);
        if(product == null)
            return NotFound($"Product with ID {id} not found");

        try
        {
            _productcontext.Products.Remove(product);
            if (await _productcontext.SaveChangesAsync() > 0)
                return Ok($"Product with id {id} deleted successfully");
            {
                return StatusCode(400, $"Product with id {id} was not deleted");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An unexpected error occurred while processing your request.");

        }
    }
    
    
    
    
    
}