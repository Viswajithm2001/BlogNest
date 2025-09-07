using Microsoft.AspNetCore.Mvc;
using BlogNest.Dtos;
using BlogNest.Services;
using BlogNest.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogNest.Controllers
{
    /// <summary>
    /// Controller responsible for handling authentication-related operations such as user registration and login.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the AuthController.
        /// </summary>
        /// <param name="authService">The authentication service for handling user authentication operations.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userRegisterDto">The registration information containing username, email, and password.</param>
        /// <returns>
        /// 200 OK with the created user information if successful,
        /// 400 Bad Request if registration data is invalid,
        /// 409 Conflict if username/email already exists,
        /// 500 Internal Server Error for unexpected errors.
        /// </returns>
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

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="userLoginDto">The login credentials containing username and password.</param>
        /// <returns>
        /// 200 OK with JWT token and username if authentication succeeds,
        /// 401 Unauthorized if credentials are invalid.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var token = await _authService.LoginAsync(userLoginDto);
            if (token == null)
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(new { Token = token, userLoginDto.Username });
        }

        /// <summary>
        /// Updates the privacy setting of the authenticated user's profile.
        /// </summary>
        /// <param name="isPublic">True to make the profile public, false to make it private.</param>
        /// <returns>
        /// 204 No Content if update succeeds,
        /// 404 Not Found if user doesn't exist,
        /// 401 Unauthorized if user is not authenticated.
        /// </returns>
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

        /// <summary>
        /// Resets a user's password without requiring authentication.
        /// </summary>
        /// <param name="dto">The password reset information containing the new password and confirmation.</param>
        /// <returns>
        /// 200 OK if password reset succeeds,
        /// 400 Bad Request if passwords don't match,
        /// 404 Not Found if user with provided email doesn't exist.
        /// </returns>
        [HttpPut("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] UpdatePasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match." });
            }
            var result = await _authService.ResetPasswordAsync(dto);
            if(!result) return NotFound(new { message = "User with the provided email does not exist." });

            return Ok(new { message = "Password reset successfully." });
        }
    }
}