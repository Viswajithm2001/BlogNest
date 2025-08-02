using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogNest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Optional: requires JWT token
    public class PostsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            // Replace this with your real logic
            var posts = new[]
            {
                new { Id = 1, Title = "First Post", Content = "This is the first post." },
                new { Id = 2, Title = "Second Post", Content = "This is the second post." }
            };
            return Ok(posts);
        }
    }
}
