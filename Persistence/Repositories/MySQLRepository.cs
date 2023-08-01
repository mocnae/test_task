using Domain;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class MySQLCategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public MySQLCategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category entity)
        {
            await _context.Set<Category>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Set<Category>().FindAsync(id);
            _context.Set<Category>().Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Set<Category>().ToListAsync();
        }

        public async Task<Category> GetDetailAsync(int id)
        {
            return await _context.Set<Category>().FindAsync(id);
        }

        public async Task UpdateAsync(int id, Category entity)
        {
            var category = await _context.Set<Category>().FindAsync(id);
            category.Description = entity.Description ?? category.Description;
            await _context.SaveChangesAsync();
        }
    }

    public class MySQLProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public IMapper _mapper;

        public MySQLProductRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(Product entity)
        {
            await _context.Set<Product>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Set<Product>().FindAsync(id);
            _context.Set<Product>().Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Set<Product>().ToListAsync();
        }

        public async Task<Product> GetDetailAsync(int id)
        {
            return await _context.Set<Product>().FindAsync(id);
        }

        public async Task UpdateAsync(int id, Product entity)
        {
            var product = await _context.Set<Product>().FindAsync(id);
            entity.Id = id;
            _mapper.Map(entity, product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(int id)
        {
            var products = await _context.Set<Product>()
                .Where(p => p.CategoryId == id)
                .ToListAsync();
            
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string s)
        {
            var products = await _context.Set<Product>()
                .Where(p => p.Name.Contains(s))
                .ToListAsync();
            
            return products;
        }
    }
}