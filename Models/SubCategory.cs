using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tinytots.Models;

public class SubCategory
{ 
    [Key] public int SubCategoryId  { get; set; }
    [MaxLength (15)]  
    public required string Name { get; set; }
    public int CategoryId { get; init; }
    public Category Category { get; set; } = null!;
}