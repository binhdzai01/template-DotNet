using Microsoft.AspNetCore.Mvc;

namespace Template.Schedule.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Test")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
