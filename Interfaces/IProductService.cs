using Tinytots.Models;

namespace Tinytots.Interfaces;

public interface IProductService
{
   Task<List<Product>> GetAllProducts(); 
   Task<List<Product>> GetAllAvailableProducts(); 
   Task<Product> GetProductById(int id);
   Task<Product> GetProductByName(string name);
   Task<IEnumerable<Product>> GetProductByAge(int age);
   Task<IEnumerable<Product>> GetProductByGender(string Gender);
   Task<IEnumerable<Product>> GetProductByCategory(int categoryId);
   Task<IEnumerable<Product>> GetProductBySubCategory(int SubCategoryId);
   Task<Product> CreateProduct(Product product);
   Task<Product> UpdateProduct(Product product);
   Task<bool> DeleteProduct(int id);
}