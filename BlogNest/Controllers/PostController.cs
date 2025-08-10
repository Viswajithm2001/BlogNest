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
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetAllPosts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Tags)
                .Where(p => p.User.IsPublic || p.UserId.ToString() == userId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var normalizedSearch = search.Trim().ToLowerInvariant();
                query = query.Where(p =>
                    p.Title.ToLower().Contains(normalizedSearch) ||
                    p.Content.ToLower().Contains(normalizedSearch) ||
                    p.Tags.Any(t => t.Name.ToLower().Contains(normalizedSearch)));
            }

            var totalItems = await query.CountAsync();

            var posts = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PostResponseDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UserId = p.UserId,
                    AuthorUsername = p.User.Username,
                    IsAuthorPublic = p.User.IsPublic,
                    Tags = p.Tags.Select(t => t.Name).ToList(),
                    Comments = p.Comments
                        .OrderByDescending(c => c.CreatedAt)
                        .Select(c => new CommentResponseDto
                        {
                            Id = c.Id,
                            Content = c.Content,
                            CreatedAt = c.CreatedAt,
                            PostId = c.PostId,
                            UserId = c.UserId,
                            AuthorUsername = c.User.Username
                        }).ToList()
                })
                .ToListAsync();

            var response = new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Posts = posts
            };

            return Ok(response);
        }




        [HttpGet("user/{accountId:guid}")]
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetPostsByUser(Guid accountId)
        {
            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == accountId);

            if (user == null)
                return NotFound("User not found");

            if (!user.IsPublic && requestingUserId != accountId.ToString())
                return Forbid();

            var posts = await _dbContext.Posts
                .Where(p => p.UserId == accountId)
                .Select(p => new PostResponseDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UserId = p.UserId,
                    AuthorUsername = user.Username,
                    IsAuthorPublic = user.IsPublic,
                    Tags = p.Tags.Select(t => t.Name).ToList()
                })
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return Ok(posts);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PostResponseDto>> GetPostById(Guid id)
        {
            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = await _dbContext.Posts
                .Include(p => p.User).Include(p => p.Comments)
                .Where(p => p.Id == id)
                .Select(p => new PostResponseDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UserId = p.UserId,
                    AuthorUsername = p.User.Username,
                    IsAuthorPublic = p.User.IsPublic,
                    Tags = p.Tags.Select(t => t.Name).ToList(),
                    Comments = p.Comments.Select(c => new CommentResponseDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        PostId = c.PostId,
                        UserId = c.UserId,
                        AuthorUsername = c.User.Username
                    }).OrderByDescending(c => c.CreatedAt).ToList()

                })
                .FirstOrDefaultAsync();

            if (post == null)
                return NotFound();

            if (!post.IsAuthorPublic && post.UserId.ToString() != requestingUserId)
                return Unauthorized("This account is private.");

            return Ok(post);
        }

        [HttpGet("tag/{tagName}")]
        public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetPostsByTag(string tagName)
        {
            var normalized = tagName.Trim().ToLowerInvariant();

            var posts = await _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Tags)
                .Include(p => p.Comments) // if you want comments too
                    .ThenInclude(c => c.User)
                .Where(p => p.Tags.Any(t => t.Name == normalized) && (p.User.IsPublic || p.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier)))
                .Select(p => new PostResponseDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UserId = p.UserId,
                    AuthorUsername = p.User.Username,
                    Tags = p.Tags.Select(t => t.Name).ToList(),
                    Comments = p.Comments.Select(c => new CommentResponseDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        PostId = c.PostId,
                        UserId = c.UserId,
                        AuthorUsername = c.User.Username
                    }).ToList()
                })
                .ToListAsync();

            return Ok(posts);
        }

        [HttpPost]
        public async Task<ActionResult<PostResponseDto>> CreatePost([FromBody] CreatePostDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("Invalid post data.");

            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var userId))
                return Unauthorized();

            // Normalize tag names: trim, to-lower, unique
            var incomingTagNames = (request.Tags ?? new List<string>())
                .Select(t => t?.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t!.ToLowerInvariant())
                .Distinct()
                .ToList();

            // Pre-load existing tags matching incoming (single query)
            List<Tag> existingTags = new();
            if (incomingTagNames.Count > 0)
            {
                // Note: using ToLower on DB side can be slower; alternative: store tags in lowercase always
                existingTags = await _dbContext.Tags
                    .Where(t => incomingTagNames.Contains(t.Name.ToLower()))
                    .ToListAsync();
            }

            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                Tags = new List<Tag>()
            };

            foreach (var name in incomingTagNames)
            {
                var tag = existingTags.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (tag != null)
                {
                    post.Tags.Add(tag);
                }
                else
                {
                    var newTag = new Tag { Id = Guid.NewGuid(), Name = name };
                    post.Tags.Add(newTag);
                    _dbContext.Tags.Add(newTag); // ensure it will be inserted
                }
            }

            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();

            var resp = new PostResponseDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UserId = post.UserId,
                AuthorUsername = (await _dbContext.Users.FindAsync(userId))?.Username ?? "",
                Tags = post.Tags.Select(t => t.Name).ToList()
            };

            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, resp);
        }

        // PUT: api/posts/{id}
        // TODO: Add [Authorize] and owner-check logic later
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostDto request)
        {
            var post = await _dbContext.Posts
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();

            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.UserId.ToString() != requestingUserId) return Forbid();

            if (!string.IsNullOrWhiteSpace(request.Title)) post.Title = request.Title;
            if (!string.IsNullOrWhiteSpace(request.Content)) post.Content = request.Content;

            if (request.Tags != null)
            {
                var normalized = request.Tags
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Select(t => t!.Trim().ToLowerInvariant())
                    .Distinct()
                    .ToList();

                // Load all existing tags used in normalized list
                var existing = await _dbContext.Tags
                    .Where(t => normalized.Contains(t.Name.ToLower()))
                    .ToListAsync();

                // Remove tags that are not in new normalized set
                var toRemove = post.Tags.Where(t => !normalized.Contains(t.Name.ToLower())).ToList();
                foreach (var r in toRemove) post.Tags.Remove(r);

                // Add new tags
                foreach (var name in normalized)
                {
                    if (post.Tags.Any(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                        continue;

                    var tag = existing.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                    if (tag != null)
                    {
                        post.Tags.Add(tag);
                    }
                    else
                    {
                        var newTag = new Tag { Id = Guid.NewGuid(), Name = name };
                        post.Tags.Add(newTag);
                        _dbContext.Tags.Add(newTag);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("{postId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage(Guid postId, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image file provided.");

            var post = await _dbContext.Posts.FindAsync(postId);
            if (post == null)
                return NotFound("Post not found.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.UserId.ToString() != userId)
                return Forbid();

            // Validate file type (optional)
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(image.ContentType))
                return BadRequest("Unsupported image format.");

            // Create unique filename and save path
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Save relative image URL to post
            post.ImageUrl = $"/uploads/{fileName}";
            await _dbContext.SaveChangesAsync();

            return Ok(new { imageUrl = post.ImageUrl });
        }

        // DELETE: api/posts/{id}
        // TODO: Add [Authorize] and owner-check logic later
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _dbContext.Posts.FindAsync(id);
            if (post == null) return NotFound();
            var requestingUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.UserId.ToString() != requestingUserId) return Forbid();
            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
