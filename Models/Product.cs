using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Tinytots.Models;

public class Product
{
    [Key] public int ProductId { get; set; }
    [MaxLength(25)] public required string Name { get; set; }

    public int SubCategoryId { get; set; }
    public SubCategory SubCategory { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }

    [Required, Precision(10, 2)]
    public decimal UnitPrice { get; set; } = 0;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
