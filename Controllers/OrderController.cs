using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinytots.DbContext;
using Tinytots.DTO;
using Tinytots.Models;


namespace Tinytots.Controllers
 {
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly TinytotsDbContext  _context;

        public OrderController(TinytotsDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder(OrderCreateDTO order)
        {
           
            try
            { 
                //Find the product we're ordering by id
                var product = await _context.Products.FindAsync(order.ProductId); 
                
                if (product == null)
                {
                    return NotFound("Product not found");
                }
                //if it exists, create a new order
                var newOrder = new Order().CreateOrder(product , order.Quantity);
                await _context.Orders.AddAsync(newOrder);
                var req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return Ok(new
                    {
                        message = $"Order for {product.Name} has been created successfully",
                        Data = newOrder
                    });
                }
                
                return BadRequest("Order not created");
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // [HttpPatch("Update/{OrderUpdateDTO.OrderId}")]
        // public async Task<IActionResult> UpdateOrder(OrderUpdateDTO order)
        // {
        //     try
        //     {
        //        var _order = await _context.Orders.FindAsync(order.OrderId);
        //        if (_order == null)
        //        {
        //            return NotFound("Order not found");
        //        }
        //        
        //        _order.Quantity = order.Quantity;
        //        _order.LinePrice = order.Quantity * _order.UnitPrice;
        //        
        //        var req = await _context.SaveChangesAsync();
        //        if (req > 0)
        //        {
        //            return Ok("Order Quantity updated successfully");
        //        } 
        //        return BadRequest("Order not updated");
        //     }
        //     
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         throw;
        //     }
        // }
        //
        //
        
    }
}