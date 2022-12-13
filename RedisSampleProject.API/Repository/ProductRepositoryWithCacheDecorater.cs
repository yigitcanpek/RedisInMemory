using RedisSampleProject.API.Models;
using RedisSampleProject.RedisCache;
using StackExchange.Redis;
using System.Text.Json;
//decorater design pattern
namespace RedisSampleProject.API.Repository
{
    public class ProductRepositoryWithCacheDecorater : IProductRepository
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheDatabase;

        public ProductRepositoryWithCacheDecorater(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheDatabase = _redisService.Getdb(2);
        }

        //Private Methods

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAsync();
            products.ForEach(p =>
            {
                _cacheDatabase.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });
            return products;
        }
        public async Task<Product> CreateAsync(Product product)
        {
            Product newProduct = await _productRepository.CreateAsync(product);
            
            if (await _cacheDatabase.KeyExistsAsync(productKey))
            {
                await _cacheDatabase.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newProduct));
            }
            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            if (!await _cacheDatabase.KeyExistsAsync(productKey))
            {
                return await LoadToCacheFromDbAsync();
            }
            List<Product> products = new List<Product>();
            HashEntry[] cacheProducts = await _cacheDatabase.HashGetAllAsync(productKey);
            foreach (HashEntry item in cacheProducts.ToList() )
            {
                Product product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }
            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (_cacheDatabase.KeyExists(productKey))
            {
                RedisValue product = await _cacheDatabase.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) :null;
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);
        }
    }
}
