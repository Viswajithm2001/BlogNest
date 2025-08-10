using Microsoft.AspNetCore.Mvc;

namespace BlogNest.Controllers
{
    public class BlogController : ControllerBase
    {
        private readonly ILogger<BlogController> _logger;

        public BlogController(ILogger<BlogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetPosts()
        {
            // Logic to retrieve posts
            _logger.LogInformation("Retrieving all posts.");
            return Ok(new { Message = "List of posts" });
        }
    }
}