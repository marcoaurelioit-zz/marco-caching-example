using System;
using System.Threading.Tasks;
using Marco.Caching.Example.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marco.Caching.Example.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ICache cache;

        public ProductsController(ICache cache)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        [HttpGet("{id}")]
        public async Task<Product> GetByIdAsync(int id)
        {
            var cacheKey = $"{nameof(Product)}:{id}";

            if (!cache.TryGetValue<Product>(cacheKey, out var product))
            {
                product = new Product { Id = id, Title = $"Product {id}", Price = 2.5m }; // Fake Product
                await cache.SetAsync(cacheKey, product);
            }

            return product;
        }
    }
}