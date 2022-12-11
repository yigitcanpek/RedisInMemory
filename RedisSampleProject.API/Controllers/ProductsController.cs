using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using RedisSampleProject.API.Models;
using RedisSampleProject.API.Repository;
using RedisSampleProject.RedisCache;
using StackExchange.Redis;

namespace RedisSampleProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
     
        private readonly IProductRepository _productRepository;
        private readonly StackExchange.Redis.IDatabase _database;
        public ProductsController(IProductRepository productRepository, RedisService redisService, StackExchange.Redis.IDatabase database)
        {
            _productRepository = productRepository;
            _database = database;
            _database.StringSet("category", "movie");
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepository.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productRepository.GetByIdAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult>Create(Product product)
        {
            return Created(string.Empty, await _productRepository.CreateAsync(product));
        }
    }
}
