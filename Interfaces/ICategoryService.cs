using Tinytots.Models;

namespace Tinytots.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategories();
    Task<Category> CreateCategory(Category category); 
    Task<Category> GetCategoryByName(string name);
    Task<Category> UpdateCategory(Category category);
    Task<bool>DeleteCategoryByName(string name);

}