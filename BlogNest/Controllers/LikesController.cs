using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogNest.Data;
using BlogNest.Dtos;
using BlogNest.Models;
using System.Security.Claims;
namespace BlogNest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Optional: requires JWT token
    public class LikesController : ControllerBase
    {
        private readonly BlogDbContext _dbContext;

        public LikesController(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("like/{postId}")]
        public async Task<IActionResult> LikePost(Guid postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _dbContext.Users.FindAsync(Guid.Parse(userId));

            if (user == null)
                return NotFound("User not found.");

            var post = await _dbContext.Posts.FindAsync(postId);
            if (post == null)
                return NotFound("Post not found.");

            var existingLike = _dbContext.Likes
                .FirstOrDefault(l => l.PostId == postId && l.UserId == user.Id);

            if (existingLike != null)
                return BadRequest("You have already liked this post.");

            var like = new Like
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                UserId = user.Id,
            };

            _dbContext.Likes.Add(like);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Post liked successfully." });
        }
        [HttpDelete("unlike/{postId}")]
        public async Task<IActionResult> UnlikePost(Guid postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var like = _dbContext.Likes
                .FirstOrDefault(l => l.PostId == postId && l.UserId == Guid.Parse(userId));

            if (like == null)
                return NotFound("Like not found for this post.");

            _dbContext.Likes.Remove(like);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Post unliked successfully." });
        }

        [HttpGet("count/{postId}")]
        [AllowAnonymous] // Optional: anyone can see like count
        public IActionResult GetLikeCount(Guid postId)
        {
            var likeCount = _dbContext.Likes.Count(l => l.PostId == postId);
            return Ok(new { postId, likeCount });
        }


    }
}