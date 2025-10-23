using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Tinytots.Services;

namespace Tinytots.Models;

public class Order
{
   [Key] public int OrderId { get; init; }

    public string OrderCode { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Name { get; set; } = null!;
    
    public int ProductId { get; init; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    
    [Precision(10,2)]
    public decimal UnitPrice { get; set; }  
    
    [Precision(20,2)]
    public decimal LinePrice { get; set; }  
    
    public DateTimeOffset CreatedAt  = DateTimeOffset.UtcNow;

    public int? InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }

    public string UniqueOrderCode()
    {
        var today = DateTime.Now.ToString("yyyyMMdd");
        var randomNum = RandomNumberGenerator.GetInt32(100, 100000).ToString("D5");
        var code = $"TTORD-{today}-{randomNum}";

        return code;
    }


   public Order CreateOrder(Product product, int quantity)
   {
       return new Order
       {
           OrderCode = UniqueOrderCode(),
           Name = product.Name,
           ProductId = product.ProductId,
           Quantity = quantity,
           UnitPrice = product.UnitPrice,
           LinePrice = quantity * product.UnitPrice
       };
   }
}
