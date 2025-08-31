using Tinytots.Models;

namespace Tinytots.Interfaces;

public interface ISubCategoryService
{
    Task<IEnumerable<SubCategory>> GetAllSubCategories();
    Task<SubCategory> CreateSubCategory(SubCategory subCategory);
    Task<SubCategory> GetSubCategoryByName(string name);
    Task<SubCategory> UpdateSubCategory(SubCategory subCategory);
    Task<bool> DeleteSubCategoryByName(string name);
}