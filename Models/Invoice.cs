namespace Tinytots.Models;

public class Invoice
{
    public int Id { get; set; }
    public string Code { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}