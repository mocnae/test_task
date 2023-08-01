using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
using Serilog;
using System.Linq;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            _logger.LogInformation("Request information GetProducts");
            return Ok(await _productRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductDetails(int id)
        {
            return Ok(await _productRepository.GetDetailAsync(id));
        }

        [Authorize(Roles = "user, advanced")]
        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            await _productRepository.AddAsync(product);
            return CreatedAtAction(nameof(GetProductDetails), new { id = product.Id }, product);
        }

        [Authorize(Roles = "user, advanced")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Editproduct(int id, Product product)
        {
            await _productRepository.UpdateAsync(id, product);
            return Ok();
        }

        [Authorize(Roles = "user, advanced")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("prods/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(int id)
        {
            var products = await _productRepository.GetProductsByCategory(id);

            return Ok(products);
        }

        [HttpGet("find/{s}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string s)
        {
            var products = await _productRepository.GetProductsByName(s);

            return Ok(products);
        }
    }
}