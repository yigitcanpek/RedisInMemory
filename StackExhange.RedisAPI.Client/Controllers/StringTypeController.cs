using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExhange.RedisAPI.Web.Services;

namespace StackExhange.RedisAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            redisService.Connect();
            _database = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            
            _database.StringSet("name", "yigitcanpek");
            _database.StringSet("bebek", 100);
            return View();
        }
        public IActionResult Show()
        {
           RedisValue value = _database.StringGet("name");
            _database.StringIncrement("bebek", 1);
            Int64 count = _database.StringDecrementAsync("bebek", 1).Result;
            /* if u not wait result this way*/
            _database.StringDecrementAsync("bebek", 10).Wait();

            Byte[] imagebyte=default(byte[]);
            _database.StringSet("image", imagebyte);
            RedisValue valuerange = _database.StringGetRange("name", 0, 3);
            RedisValue valuelenght = _database.StringLength("name");
            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
            }
            return View();
        }
    }
}
