using Microsoft.AspNetCore.Mvc;
using Template.PubSub.Services;

namespace Template.PubSub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : ControllerBase
    {

        private readonly ILogger<RedisController> _logger;
        private readonly ICacheService _cacheService;

        public RedisController(ILogger<RedisController> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        [HttpPost("insert-update-redis")]
        public IActionResult InsertUpdateRedis([FromBody] string key, string value)
        {
            _ = _cacheService.SetAsync(key, value);
            return Ok();
        }

        [HttpGet("get-value")]
        public async Task<string> GetValueCache([FromQuery] string key)
        {
            var value = await _cacheService.GetAsync<string>(key);
            return string.IsNullOrEmpty(value) ? "Not Found" : value;
        }

        [HttpGet("get-list-key")]
        public async Task<List<string>> GetListKey()
        {
            var value = await _cacheService.GetListKey("*");
            return value;
        }
    }
}
