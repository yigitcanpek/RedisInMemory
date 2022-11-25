using InMemory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            //1st way
            /*if (String.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            {
                _memoryCache.Set<string>("time", DateTime.Now.ToString()); //Key,Value
            }
            */
            //2nd way
            if (!_memoryCache.TryGetValue("time",out string timecache))
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(30);
                options.SlidingExpiration = TimeSpan.FromSeconds(10);
                options.Priority=CacheItemPriority.High;//Priority
                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _memoryCache.Set("callback", $"{key}->{value}=>sebep:{reason}");
                }); //call back deleted data
                _memoryCache.Set<string>("time", DateTime.Now.ToString(),options); //Key,Value
            }

            Product p = new Product { Id = 1, Name = "Kalem", Price = 200 };
            _memoryCache.Set<Product>("product:1", p);

            return View();
        }
        public IActionResult Show()
        {
            _memoryCache.TryGetValue("time", out string timecache);
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.time = timecache;
            ViewBag.callback = callback;
            ViewBag.product = _memoryCache.Get<Product>("product:1");
            //ViewBag.time=_memoryCache.Get<string>("time");
            //_memoryCache.Remove("time");

            /*_memoryCache.GetOrCreate<string>("time", entry =>
            {
            return DateTime.Now.ToString();
            });*/
            return View();
        }
    }
}
