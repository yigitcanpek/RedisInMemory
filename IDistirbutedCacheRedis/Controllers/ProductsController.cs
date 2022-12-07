using IDistirbutedCacheRedis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace IDistirbutedCacheRedis.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            //_distributedCache.SetString("name", "Yiğit", cacheEntryOptions);

            //Caching with complex data types 
            Product product = new Product{ Id=1 , Name="pen",Price=100};
            Product productnd = new Product { Id = 2, Name = "pen", Price = 100 };
            string jsonproduct=JsonConvert.SerializeObject(product);
            string jsonproductnd = JsonConvert.SerializeObject(productnd);


            Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct);
            Byte[] byteproductnd = Encoding.UTF8.GetBytes(jsonproductnd);

            _distributedCache.Set($"product:{product.Id}", byteproduct);
            _distributedCache.Set($"product:{productnd.Id}", byteproductnd);
             //await _distributedCache.SetStringAsync($"product:{product.Id}", jsonproduct, cacheEntryOptions);
             //await _distributedCache.SetStringAsync($"product:{productnd.Id}", jsonproductnd, cacheEntryOptions);
            return View();
        }
        public IActionResult Show()
        {
            //string name = _distributedCache.GetString("name");
            //ViewBag.name = name;
            Byte[] byteproduct = _distributedCache.Get("product:1");
            Byte[] byteproductnd = _distributedCache.Get("product:2");

            string jsonproduct = Encoding.UTF8.GetString(byteproduct);
            string jsonproductnd = Encoding.UTF8.GetString(byteproductnd);

            /* Best Way*/
            //string jsonproduct = _distributedCache.GetString("product:1");
            //string jsonproductnd = _distributedCache.GetString("product:2");

            Product product = JsonConvert.DeserializeObject<Product>(jsonproduct);
            Product productnd = JsonConvert.DeserializeObject<Product>(jsonproductnd);
            ViewBag.product = product;
            ViewBag.productnd = productnd;
            return View();
        }
        public IActionResult Delete()
        {
            _distributedCache.Remove("name");
            return View();
        }

        public async Task<IActionResult> AsyncIndex()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            await _distributedCache.SetStringAsync("surname", "Pekgüzel",cacheEntryOptions);
            return View();
        } 

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Stella1.png");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("image", imageByte);
            return View();
        }
        public IActionResult ImageUrl()
        {
            byte[] imagebyte = _distributedCache.Get("image");
            return File(imagebyte,"image/png");
        }
    }
}
