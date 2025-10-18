using Tinytots.Enums;
using Tinytots.Models;

namespace Tinytots.DTO;

public class ProductDTO
{
    
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string SubCategoryName { get; set; }
    
}   