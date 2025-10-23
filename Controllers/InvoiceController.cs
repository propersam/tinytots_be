using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tinytots.DbContext;
using Tinytots.DTO;
using Tinytots.Enums;
using Tinytots.Models;

namespace Tinytots.Controllers
 {
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly TinytotsDbContext _context;

        public InvoiceController(TinytotsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                var invoices = await _context.Invoices
                    .Include(i => i.Orders)
                        .ThenInclude(o => o.Product)
                    .ToListAsync();

                return Ok(invoices);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(int id)
        {
            try
            {
                var invoice = await _context.Invoices
                    .Include(i => i.Orders)
                        .ThenInclude(o => o.Product)
                    .FirstOrDefaultAsync(i => i.InvId == id);

                if (invoice == null)
                {
                    return NotFound($"Invoice with Id {id} was not found");
                }

                return Ok(invoice);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice(InvoiceDTO invoiceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (invoiceDto.Items == null || invoiceDto.Items.Count == 0)
                {
                    return BadRequest("Invoice must contain at least one order item");
                }

                var orders = new List<Order>();
                decimal totalBill = 0;

                // Process each order item
                foreach (var item in invoiceDto.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null)
                    {
                        return NotFound($"Product with Id {item.ProductId} was not found");
                    }

                    var order = new Order().CreateOrder(product, item.Quantity);
                    orders.Add(order);
                    totalBill += order.LinePrice;
                }

                // Generate unique invoice code
                string invCode = await GenerateUniqueInvoiceCode();

                // Create invoice
                var invoice = new Invoice
                {
                    InvCode = invCode,
                    InvoiceBill = totalBill,
                    Status = StatusEnum.Pending,
                    Orders = orders
                };

                await _context.Invoices.AddAsync(invoice);
                var req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return CreatedAtAction(nameof(GetInvoiceById),
                        new { id = invoice.InvId },
                        new
                        {
                            Message = "Invoice created successfully",
                            Invoice = invoice
                        });
                }

                return BadRequest("Invoice was not created");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateInvoiceStatus(int id, [FromBody] StatusEnum status)
        {
            try
            {
                var invoice = await _context.Invoices.FindAsync(id);

                if (invoice == null)
                {
                    return NotFound($"Invoice with Id {id} was not found");
                }

                invoice.Status = status;
                var req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return Ok(new
                    {
                        Message = "Invoice status updated successfully",
                        Invoice = invoice
                    });
                }

                return BadRequest("Invoice status was not updated");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            try
            {
                var invoice = await _context.Invoices
                    .Include(i => i.Orders)
                    .FirstOrDefaultAsync(i => i.InvId == id);

                if (invoice == null)
                {
                    return NotFound($"Invoice with Id {id} was not found");
                }

                _context.Invoices.Remove(invoice);
                var req = await _context.SaveChangesAsync();

                if (req > 0)
                {
                    return Ok($"Invoice with Id {id} deleted successfully");
                }

                return BadRequest($"Invoice with Id {id} was not deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        private async Task<string> GenerateUniqueInvoiceCode()
        {
            string code;
            bool exists;

            do
            {
                var today = DateTime.Now.ToString("yyyyMMdd");
                var randomNum = RandomNumberGenerator.GetInt32(100, 100000).ToString("D5");
                code = $"TTINV-{today}-{randomNum}";
                exists = await _context.Invoices.AnyAsync(i => i.InvCode == code);
            }
            while (exists);

            return code;
        }
    }
 }