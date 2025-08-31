using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tinytots.Models;

public class SubCategory
{
    public int Id  { get; set; }
    
    public int CategoryId { get; init; }
    public Category? Category { get; set; }
    
  [Required, MaxLength (15)] 
  public string? Name { get; set; }
}