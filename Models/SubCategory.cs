using System.ComponentModel.DataAnnotations.Schema;

namespace Tinytots.Models;

public class SubCategory
{
    public int Id  { get; set; }
    
    [ForeignKey("CategoryId")] 
    public Category? Category { get; set; }
    
    public string Name { get; set; }
}