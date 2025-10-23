using System.ComponentModel.DataAnnotations;

namespace Tinytots.Models;

public class Category
{
    [Key] public int CategoryId { get; set; }

    [MaxLength(20)]
    public required string Name { get; set; }

    public ICollection<SubCategory>? SubCategory { get; set; }
}
