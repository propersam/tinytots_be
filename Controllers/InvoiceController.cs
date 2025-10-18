using Microsoft.AspNetCore.Mvc;
using Tinytots.DbContext;
using Tinytots.Models;

namespace Tinytots.Controllers
 {
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly TinytotsDbContext _context;

        public InvoiceController(TinytotsDbContext context)
        {
            _context = context;
        }

        // [HttpGet]
        // public async Task<IActionResult> CreateInvoice(List<Order> orders)
        // {
        //     try 
        //     {
        //         
        //         return Invoice;
        //     }
        //     
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         throw;
        //     }
        // }
        
        
        
        
        
        
        
    }
 }