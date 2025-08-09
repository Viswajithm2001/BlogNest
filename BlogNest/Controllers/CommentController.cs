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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Optional: requires JWT token
    public class CommentController : ControllerBase
    {
        private readonly BlogDbContext _dbContext;

        public CommentController(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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

        [HttpGet("post/{postId:guid}")]
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

        [HttpPut("{commentId:guid}")]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] UpdateCommentDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Invalid comment content.");
            }

            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                AuthorUsername = (await _dbContext.Users.FindAsync(comment.UserId))?.Username
            });
        }
        [HttpDelete("{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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