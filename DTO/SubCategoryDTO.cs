using System.ComponentModel.DataAnnotations;

namespace Tinytots.DTO
{
    public class SubCategoryDTO
    { 
        [Required, MaxLength (15)]  
        public string Name { get; set; }

        public string CategoryName { get; init; }
    }
}