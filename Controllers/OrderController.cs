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

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Product)
                    .Include(o => o.Invoice)
                    .ToListAsync();

                return Ok(orders);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == id);

                if (order == null)
                {
                    return NotFound($"Order with Id {id} was not found");
                }

                return Ok(order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderCreateDTO order)
        {
            try
            {
                var product = await _context.Products.FindAsync(order.ProductId);

                if (product == null)
                {
                    return NotFound($"Product with Id {order.ProductId} not found");
                }

                var newOrder = new Order().CreateOrder(product, order.Quantity);
                await _context.Orders.AddAsync(newOrder);
                var req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return CreatedAtAction(nameof(GetOrderById),
                        new { id = newOrder.OrderId },
                        new
                        {
                            Message = $"Order for {product.Name} created successfully",
                            Order = newOrder
                        });
                }

                return BadRequest("Order was not created");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderCreateDTO orderUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var order = await _context.Orders
                    .Include(o => o.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == id);

                if (order == null)
                {
                    return NotFound($"Order with Id {id} was not found");
                }

                // Update quantity and recalculate line price
                order.Quantity = orderUpdate.Quantity;
                order.LinePrice = orderUpdate.Quantity * order.UnitPrice;

                var req = await _context.SaveChangesAsync();
                if (req > 0)
                {
                    return Ok(new
                    {
                        Message = "Order updated successfully",
                        Order = order
                    });
                }

                return BadRequest("Order was not updated");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);

                if (order == null)
                {
                    return NotFound($"Order with Id {id} was not found");
                }

                _context.Orders.Remove(order);
                var req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return Ok($"Order with Id {id} deleted successfully");
                }

                return BadRequest($"Order with Id {id} was not deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }
    }
}