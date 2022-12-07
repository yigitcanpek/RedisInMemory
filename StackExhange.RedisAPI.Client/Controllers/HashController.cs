using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using StackExchange.Redis;
using StackExhange.RedisAPI.Web.Services;

namespace StackExhange.RedisAPI.Web.Controllers
{
    public class HashController : BaseController
    {
        public HashController(RedisService redisService) : base(redisService)
        {
            
        }
        public IActionResult Index()
        {
            Dictionary<string, string> hashlist = new Dictionary<string, string>();
            if (_database.KeyExists(listKey))
            {
                _database.HashGetAll(listKey).ToList().ForEach(x =>
                {
                    hashlist.Add(x.Name, x.Value);
                });
            }
            return View(hashlist);
        }
        [HttpPost]
        public IActionResult Add(string key,string value)
        {
            _database.HashSet(listKey,key,value);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteItem(string key)
        {
            _database.HashDelete(listKey,key);
            return RedirectToAction("Index");
        }
    }
    
    
}
