using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Repositories;

namespace API.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryRepository _CategoryRepository;

        public CategoryController(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }

        // [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return Ok(await _CategoryRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryDetails(int id)
        {
            return Ok(await _CategoryRepository.GetDetailAsync(id));
        }

        [Authorize(Roles = "advanced")]
        [HttpPost]
        public async Task<ActionResult> AddCategory(Category category)
        {
            await _CategoryRepository.AddAsync(category);
            return CreatedAtAction(nameof(GetCategoryDetails), new { id = category.Id }, category);
        }

        [Authorize(Roles = "advanced")]
        [HttpPut("{id}")]
        public async Task<ActionResult> EditCategory(int id, Category category)
        {
            await _CategoryRepository.UpdateAsync(id, category);
            return Ok();
        }

        [Authorize(Roles = "advanced")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _CategoryRepository.DeleteAsync(id);
            return Ok();
        }

        // [AllowAnonymous]
        // [HttpPost("prods/{id}")]
        // public async Task<ActionResult<List<Product>>> GetProductsByCategory(int id) 
        // {
        //     var category = await _CategoryRepository.GetDetailAsync(id);

        //     category.Products;
        // }
    }
}