using Microsoft.AspNetCore.Mvc;
using Tinytots.DbContext;

namespace Tinytots.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly TinytotsDbContext _ProductContext;
    
    public CategoryController(TinytotsDbContext ProductContext)
    {
        _ProductContext = ProductContext;
    }

   [HttpGet("ShowAllCategories")]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    
    
    
    
    
    
    
    
    
}