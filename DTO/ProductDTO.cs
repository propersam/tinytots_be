using Tinytots.Enums;
using Tinytots.Models;

namespace Tinytots.DTO;

public class ProductDTO
{
    public string? Name { get; set; }
    public GenderEnum Gender { get; set; }
    public AgeGroupEnum AgeGroup { get; set; }
    public int CategoryId { get; set; }
    public int SubCategoryId { get; set; }
    public decimal Price { get; set; }
    public bool OutOfStock { get; set; }
}   