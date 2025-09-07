using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogNest.Data;
using BlogNest.Dtos;
using BlogNest.Models;
using System.Security.Claims;
namespace BlogNest.Controllers
{
    /// <summary>
    /// Controller for managing post likes functionality, including adding and removing likes on posts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Optional: requires JWT token
    public class LikesController : ControllerBase
    {
        private readonly BlogDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the LikesController.
        /// </summary>
        /// <param name="dbContext">The database context for accessing like-related data.</param>
        public LikesController(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds a like to a post for the currently authenticated user.
        /// </summary>
        /// <param name="postId">The unique identifier of the post to like.</param>
        /// <returns>Returns Ok with success message if the like is added, BadRequest if already liked, or NotFound if post/user not found.</returns>
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
        /// <summary>
        /// Removes a like from a post for the currently authenticated user.
        /// </summary>
        /// <param name="postId">The unique identifier of the post to unlike.</param>
        /// <returns>Returns Ok with success message if the like is removed, or NotFound if the like doesn't exist.</returns>
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
        /// <summary>
        /// Checks if the currently authenticated user has liked a specific post.
        /// </summary>
        /// <param name="postId">The unique identifier of the post to check.</param>
        /// <returns>Returns Ok with a boolean indicating if the user has liked the post, or Unauthorized if not authenticated.</returns>
        [HttpGet("user-liked/{postId}")]
        public IActionResult IsPostLikedByUser(Guid postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            bool liked = _dbContext.Likes.Any(l => l.PostId == postId && l.UserId == Guid.Parse(userId));
            return Ok(new { liked });
        }

        /// <summary>
        /// Gets the total number of likes for a specific post.
        /// </summary>
        /// <param name="postId">The unique identifier of the post.</param>
        /// <returns>Returns Ok with the post ID and its like count.</returns>
        [HttpGet("count/{postId}")]
        [AllowAnonymous] // Optional: anyone can see like count
        public IActionResult GetLikeCount(Guid postId)
        {
            var likeCount = _dbContext.Likes.Count(l => l.PostId == postId);
            return Ok(new { postId, likeCount });
        }


    }
}