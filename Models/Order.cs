using System.ComponentModel.DataAnnotations.Schema;

namespace Tinytots.Models;

public class Order
{
    public int Id { get; set; }
    
    [ForeignKey("ProductId")] 
    public Product Product { get; set; }
    
    public string Inv_Code { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}