using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExhange.RedisAPI.Web.Services;

namespace StackExhange.RedisAPI.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly RedisService _redisService;
        protected readonly IDatabase _database;
        protected readonly string listKey = "names";
        public BaseController(RedisService redisService)
        {
            _redisService = redisService;
            redisService.Connect();
            _database = _redisService.GetDb(0);
        }
    }
}
