using System.ComponentModel.DataAnnotations;

namespace Tinytots.DTO
{
    public class SubCategoryDTO
    {
        [Required, MaxLength(15)]
        public required string Name { get; set; }

        public required string CategoryName { get; init; }
    }
}