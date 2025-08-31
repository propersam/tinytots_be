using System.ComponentModel.DataAnnotations;

namespace Tinytots.Models;

public class Category
{
    public int Id { get; set; }
   [Required, MaxLength(20)] 
   public string? CatName { get; set; }
   
   public ICollection<Product>? Products { get; set; } 

   public ICollection<SubCategory>? SubCategories { get; set; }
}