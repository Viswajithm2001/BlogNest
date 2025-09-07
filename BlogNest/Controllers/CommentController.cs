using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogNest.Data;
using BlogNest.Models;
using BlogNest.Dtos;
using System.Security.Claims;
namespace BlogNest.Controllers
{
    /// <summary>
    /// Controller for managing comments on blog posts, including creation, retrieval, updating, and deletion of comments.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Optional: requires JWT token
    public class CommentController : ControllerBase
    {
        private readonly BlogDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the CommentController.
        /// </summary>
        /// <param name="dbContext">The database context for accessing comment-related data.</param>
        public CommentController(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new comment on a blog post.
        /// </summary>
        /// <param name="request">The comment creation data containing content and post ID.</param>
        /// <returns>
        /// 201 Created with the created comment data if successful,
        /// 400 Bad Request if the comment data is invalid,
        /// 401 Unauthorized if user is not authenticated,
        /// 404 Not Found if the post or user doesn't exist.
        /// </returns>
        /// <remarks>
        /// The comment's creation timestamp and user information are automatically set based on the authenticated user.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Content) || request.PostId == Guid.Empty)
            {
                return BadRequest("Invalid comment data.");
            }

            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _dbContext.Users.FindAsync(Guid.Parse(requestingUserId));

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var post = await _dbContext.Posts.FindAsync(request.PostId);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                PostId = request.PostId,
                UserId = user.Id,
                User = user
            };
            var responseDto = new CommentResponseDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                PostId = comment.PostId,
                UserId = comment.UserId,
                AuthorUsername = user.Username
            };
            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommentsByPost), new { postId = comment.PostId }, responseDto);
        }

        /// <summary>
        /// Retrieves all comments for a specific blog post.
        /// </summary>
        /// <param name="postId">The unique identifier of the post to get comments for.</param>
        /// <returns>
        /// 200 OK with the list of comments if found,
        /// 404 Not Found if no comments exist for the post,
        /// 401 Unauthorized if user is not authenticated.
        /// </returns>
        /// <remarks>
        /// Comments are returned in descending order by creation date (newest first).
        /// Each comment includes the author's username and creation timestamp.
        /// </remarks>
        [HttpGet("{postId:guid}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetCommentsByPost(Guid postId)
        {
            var comments = await _dbContext.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User)
                .Select(c => new CommentResponseDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    PostId = c.PostId,
                    UserId = c.UserId,
                    AuthorUsername = c.User.Username
                })
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            if (!comments.Any())
                return NotFound("No comments found for this post.");

            return Ok(comments);
        }

        /// <summary>
        /// Updates an existing comment's content.
        /// </summary>
        /// <param name="commentId">The unique identifier of the comment to update.</param>
        /// <param name="request">The update data containing the new content.</param>
        /// <returns>
        /// 200 OK with the updated comment data if successful,
        /// 400 Bad Request if the comment content is invalid,
        /// 401 Unauthorized if user is not authenticated,
        /// 403 Forbidden if user is not the comment author,
        /// 404 Not Found if the comment doesn't exist.
        /// </returns>
        /// <remarks>
        /// Only the original author of the comment can update it.
        /// The creation timestamp is preserved, and only the content is updated.
        /// </remarks>
        [HttpPut("{commentId:guid}")]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] UpdateCommentDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Invalid comment content.");
            }

            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (requestingUserId == null)
            {
                return Unauthorized("User not authenticated.");
            }

            var comment = await _dbContext.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

            if (comment.UserId != Guid.Parse(requestingUserId))
            {
                return Forbid("You can only edit your own comments.");
            }

            comment.Content = request.Content;
            await _dbContext.SaveChangesAsync();

            return Ok(new CommentResponseDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                PostId = comment.PostId,
                UserId = comment.UserId,
                AuthorUsername = (await _dbContext.Users.FindAsync(comment.UserId))?.Username ?? "Unknown User"
            });
        }
        /// <summary>
        /// Deletes an existing comment.
        /// </summary>
        /// <param name="commentId">The unique identifier of the comment to delete.</param>
        /// <returns>
        /// 204 No Content if deletion is successful,
        /// 401 Unauthorized if user is not authenticated,
        /// 403 Forbidden if user is not the comment author,
        /// 404 Not Found if the comment doesn't exist.
        /// </returns>
        /// <remarks>
        /// Only the original author of the comment can delete it.
        /// Once deleted, the comment cannot be recovered.
        /// </remarks>
        [HttpDelete("{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (requestingUserId == null)
            {
                return Unauthorized("User not authenticated.");
            }
            var comment = await _dbContext.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

            if (comment.UserId != Guid.Parse(requestingUserId))
            {
                return Forbid("You can only delete your own comments.");
            }

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}