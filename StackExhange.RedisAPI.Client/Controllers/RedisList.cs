using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExhange.RedisAPI.Web.Services;

namespace StackExhange.RedisAPI.Web.Controllers
{
    public class RedisList : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private readonly string listKey = "names";
        public RedisList(RedisService redisService)
        {
            _redisService = redisService;
            redisService.Connect();
            _database = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            List<string> namelist = new List<string>();
            if (_database.KeyExists(listKey))
            {
                _database.ListRange(listKey).ToList().ForEach(x=>
                {
                    namelist.Add(x.ToString());
                });
            }
            return View(namelist);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            _database.ListRightPush(listKey, name);
           return RedirectToAction("Index");
        }
       
        public IActionResult DeleteItem(string name)
        {
            _database.ListRemoveAsync(listKey, name).Wait();
            return RedirectToAction("Index");
        }
    }
}
