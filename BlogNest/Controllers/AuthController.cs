using Microsoft.AspNetCore.Mvc;
using BlogNest.Dtos;
using BlogNest.Services;
using BlogNest.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogNest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(userRegisterDto);
                if (result == null)
                {
                    return BadRequest("Registration failed");
                }
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // Duplicate user -> 409 Conflict
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                // Unexpected error
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Registration failed" });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var token = await _authService.LoginAsync(userLoginDto);
            if (token == null)
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(new { Token = token });
        }
        [HttpPut("privacy")]
        [Authorize]
        public async Task<IActionResult> UpdatePrivacy(bool isPublic)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _authService.UpdatePrivacyAsync(Guid.Parse(userId), isPublic);
            if (!user)
            {
                return NotFound("User not found");
            }
            return NoContent();
        }

    }
}