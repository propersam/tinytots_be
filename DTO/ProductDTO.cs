using Tinytots.Enums;
using Tinytots.Models;

namespace Tinytots.DTO;

public class ProductDTO
{

        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public required string SubCategoryName { get; set; }

}   