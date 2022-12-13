using RedisSampleProject.API.Models;

namespace RedisSampleProject.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductService _productRepository;

        public ProductService(IProductService productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> CreateAsync(Product product)
        {
          return  await _productRepository.CreateAsync(product);
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _productRepository.GetAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            Product product = await _productRepository.GetByIdAsync(id);
            return product;
        }
    }
}
