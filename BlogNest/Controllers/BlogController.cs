using Microsoft.AspNetCore.Mvc;

namespace BlogNest.Controllers
{
    /// <summary>
    /// Controller for managing the main blog functionality and general blog-related operations.
    /// </summary>
    public class BlogController : ControllerBase
    {
        private readonly ILogger<BlogController> _logger;

        /// <summary>
        /// Initializes a new instance of the BlogController.
        /// </summary>
        /// <param name="logger">The logger instance for logging blog-related activities.</param>
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