using Microsoft.AspNetCore.Mvc;
using BlogNest.Dtos;
using BlogNest.Services;

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
    }
}