using System.ComponentModel.DataAnnotations;

namespace Tinytots.Models;

public class Order
{
    public int Id { get; set; }
    
    public int ProductId { get; init; }
    public Product? Product { get; set; }
    
    [MaxLength(Int32.MaxValue)]
    public string? InvCode { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
   public DateTime CreatedAt { get; init; } 
}