using System.Security.Claims;
using BlogNest.Data;
using BlogNest.Dtos;
using BlogNest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogNest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Optional: requires JWT token
    public class PostController : ControllerBase
    {
        private readonly BlogDbContext _dbContext;
        public PostController(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("default")]
        public IActionResult GetDefault()
        {
            // Replace this with your real logic
            var posts = new[]
            {
                new { Id = 1, Title = "First Post", Content = "This is the first post." },
                new { Id = 2, Title = "Second Post", Content = "This is the second post." }
            };
            return Ok(posts);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetAllPosts()
        {
            var posts = await _dbContext.Posts.OrderByDescending(post => post.CreatedAt).Select(post => new PostResponseDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UserId = post.UserId
            }).ToListAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostResponseDto>> GetPostById(Guid id)
        {
            var post = await _dbContext.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            var postDto = new PostResponseDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UserId = post.UserId
            };
            return Ok(postDto);
        }
        [HttpPost]
        public async Task<ActionResult<PostResponseDto>> CreatePost([FromBody] CreatePostDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Invalid post data.");
            }
            Guid userId;
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(claim) && Guid.TryParse(claim, out var parsed))
            {
                userId = parsed;
            }
            else if (request.UserId != Guid.Empty)
            {
                userId = request.UserId; // fallback for now
            }
            else
            {
                return BadRequest(new { message = "UserId is required (authenticate to avoid sending it explicitly)." });
            }

            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow, // set CreatedAt here
                UserId = userId
            };

            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();

            var resp = new PostResponseDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UserId = post.UserId
            };

            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, resp);
        }
        // PUT: api/posts/{id}
        // TODO: Add [Authorize] and owner-check logic later
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostDto request)
        {
            var post = await _dbContext.Posts.FindAsync(id);
            if (post == null) return NotFound();

            // update fields (partial update allowed)
            if (!string.IsNullOrEmpty(request.Title)) post.Title = request.Title;
            if (!string.IsNullOrEmpty(request.Content)) post.Content = request.Content;

            _dbContext.Posts.Update(post);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        // DELETE: api/posts/{id}
        // TODO: Add [Authorize] and owner-check logic later
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _dbContext.Posts.FindAsync(id);
            if (post == null) return NotFound();

            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
