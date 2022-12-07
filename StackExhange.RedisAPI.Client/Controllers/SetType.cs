using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExhange.RedisAPI.Web.Services;

namespace StackExhange.RedisAPI.Web.Controllers
{
    public class SetType : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private readonly string listKey = "names";
        public SetType(RedisService redisService)
        {
            _redisService = redisService;
            redisService.Connect();
            _database = _redisService.GetDb(3);
        }
        public IActionResult Index()
        {
            HashSet<string> nameList = new HashSet<string>();

            if (_database.KeyExists(listKey))
            {
                _database.SetMembers(listKey).ToList().ForEach(x =>
                {
                    nameList.Add(x);
                });
            }

            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
        //    if (!_database.KeyExists(listKey))
        //    {
        //        _database.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
        //    }

            _database.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

            _database.SetAdd(listKey, name);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string name)
        {
            await _database.SetRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
