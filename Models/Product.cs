using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Tinytots.Enums;

namespace Tinytots.Models;

public class Product
{
     public int Id { get; set; }
    [Required(ErrorMessage = "Product name is required"), MaxLength (25)] 
    public string? Name { get; set; }
    public GenderEnum Gender { get; set; }
    [Required] public AgeGroupEnum AgeGroup { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public int SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }

    [Required] public bool OutOfStock { get; set; }
    [Required, Precision (10, 2)]  public decimal Price { get; set; }
    public DateTime CreatedAt { get; init; } 
}