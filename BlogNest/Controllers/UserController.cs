using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogNest.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BlogNest.Dtos;

namespace BlogNest.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations, including profile management and user settings.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // user must be logged in
    public class UserController : ControllerBase
    {
        private readonly BlogDbContext _context;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Initializes a new instance of the UserController.
        /// </summary>
        /// <param name="context">The database context for accessing user-related data.</param>
        /// <param name="env">The web host environment for managing user-related files.</param>
        public UserController(BlogDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// <summary>
        /// Retrieves the profile information of the currently authenticated user.
        /// </summary>
        /// <returns>Returns Ok with user details if found, Unauthorized if not authenticated, or NotFound if user doesn't exist.</returns>
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
                })
                .FirstOrDefaultAsync();

            if (user == null) return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Updates the profile information of the currently authenticated user.
        /// </summary>
        /// <param name="dto">The DTO containing the updated user information (username, email, and public status).</param>
        /// <returns>Returns Ok with updated user details if successful, Unauthorized if not authenticated, or NotFound if user doesn't exist.</returns>
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
                user.IsPublic
            });
        }
    }
}