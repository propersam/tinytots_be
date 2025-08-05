using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Tinytots.Models;

public class Product
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string Gender { get; set; }
    public string AgeGroup { get; set; }
    
    [ForeignKey("CategoryId")]      
    public Category? Category { get; set; }
    
    public string? OutOfStock { get; set; }
    public decimal? Price { get; set; }
}