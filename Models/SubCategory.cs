using System.ComponentModel.DataAnnotations;

namespace Tinytots.Models;

public class SubCategory
{
    [Key] public int SubCategoryId { get; init; }

    [MaxLength(15)]
    public required string Name { get; set; }
    public int CategoryId { get; init; }
    public Category Category { get; init; } = null!;
}
