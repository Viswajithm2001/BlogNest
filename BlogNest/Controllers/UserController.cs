using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogNest.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BlogNest.Dtos;

namespace BlogNest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // user must be logged in
    public class UserController : ControllerBase
    {
        private readonly BlogDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UserController(BlogDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == Guid.Parse(userId))
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.ProfilePictureUrl
                })
                .FirstOrDefaultAsync();

            if (user == null) return NotFound();

            return Ok(user);
        }
        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var guidId = Guid.Parse(userId);
            var user = await _context.Users.FindAsync(guidId);
            if (user == null) return NotFound();

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(dto.Username))
                user.Username = dto.Username;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (dto.IsPublic.HasValue)
                user.IsPublic = dto.IsPublic.Value;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.ProfilePictureUrl,
                user.IsPublic
            });
        }

        [HttpPost("upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var guidId = Guid.Parse(userId);
            var user = await _context.Users.FindAsync(guidId);
            if (user == null) return NotFound();

            if (profilePicture != null && profilePicture.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(profilePicture.FileName)}";
                var filePath = Path.Combine("wwwroot/profile-pictures", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                user.ProfilePictureUrl = $"/profile-pictures/{fileName}";
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.ProfilePictureUrl,
                    user.IsPublic
                });
            }

            return BadRequest("Invalid file upload");
        }

    }
}