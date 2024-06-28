using Microsoft.AspNetCore.Mvc;
using Template.PubSub.Services;

namespace Template.PubSub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IPubSubService _pubSubService;

        public HomeController(ILogger<HomeController> logger, IPubSubService pubSubService)
        {
            _logger = logger;
            _pubSubService = pubSubService;
        }

        [HttpGet(Name = "test-pub-sub")]
        public IActionResult TestPubsub()
        {
            _pubSubService.PublishSystem("test-pubsub");
            return Ok();
        }
    }
}
