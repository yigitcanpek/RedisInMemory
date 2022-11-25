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
            _memoryCache.Set<string>("time", DateTime.Now.ToString()); //Key,Value
            return View();
        }
        public IActionResult Show()
        {
            ViewBag.time=_memoryCache.Get<string>("time");
            return View(Show);
        }
    }
}
