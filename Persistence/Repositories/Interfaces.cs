using Domain;

namespace Persistence.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetDetailAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product entity);
        Task UpdateAsync(int id, Product entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategory(int id);
        Task<IEnumerable<Product>> GetProductsByName(string s);
    }

    public interface ICategoryRepository
    {
        Task<Category> GetDetailAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task AddAsync(Category entity);
        Task UpdateAsync(int id, Category entity);
        Task DeleteAsync(int id);
    }
}