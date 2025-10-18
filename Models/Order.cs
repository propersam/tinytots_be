using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Tinytots.Services;

namespace Tinytots.Models;

public class Order
{
   [Key] public int OrderId { get; init; }
   
    public string OrderCode { get; set; }
   
    [MaxLength(20)]
    public string Name { get; set; } = null!;
    
    public int ProductId { get; init; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    
    [Precision(10,2)]
    public decimal UnitPrice { get; set; }  
    
    [Precision(20,2)]
    public decimal LinePrice { get; set; }  
    
    public decimal Bill { get; set; }
    
    public DateTimeOffset CreatedAt  = DateTimeOffset.UtcNow;
    
    public List<Order> Orders { get; set; } = new();
    
    public string UniqueOrderCode()
    {
        string code;
        bool exists;

        // do
        //{
            var today = DateTime.Now.ToString("yyyyMMdd");
            var randomNum = RandomNumberGenerator.GetInt32(100, 100000).ToString("D5");
            code = $"TTINV-{today}-{randomNum}";
            // exists = await _context.Invoices.AnyAsync(i => i.InvCode == code);
        //} 
        // while (exists);

        return code;
    }


   public Order CreateOrder(Product product, int quantity)
   {
       return new Order
       {
           OrderCode = UniqueOrderCode(),
           Name = product.Name,
           Quantity = quantity,
           UnitPrice = product.UnitPrice,
           LinePrice = quantity * product.UnitPrice,
           Bill = LinePrice * Orders.Count
       };
   }
}
