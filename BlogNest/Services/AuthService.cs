using BlogNest.Data;
using BlogNest.Dtos;
using BlogNest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace BlogNest.Services
{
    public interface IAuthService
    {
    /// <summary>
    /// Registers a new user with the provided registration details.
    /// </summary>
    /// <param name="userRegisterDto">The DTO containing user registration information.</param>
    /// <returns>Returns a UserResponseDto with the registered user's details.</returns>
    Task<UserResponseDto> RegisterAsync(UserRegisterDto userRegisterDto);

    /// <summary>
    /// Authenticates a user and returns a JWT token if successful.
    /// </summary>
    /// <param name="userLoginDto">The DTO containing user login credentials.</param>
    /// <returns>Returns a JWT token string if login is successful, otherwise null.</returns>
    Task<string> LoginAsync(UserLoginDto userLoginDto);

    /// <summary>
    /// Updates the privacy setting of a user account.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="isPublic">The new privacy status.</param>
    /// <returns>Returns true if the update was successful, false otherwise.</returns>
    Task<bool> UpdatePrivacyAsync(Guid userId, bool isPublic);

    /// <summary>
    /// Resets the password for a user identified by email.
    /// </summary>
    /// <param name="dto">The DTO containing the email and new password.</param>
    /// <returns>Returns true if the password was reset successfully, false otherwise.</returns>
    Task<bool> ResetPasswordAsync(UpdatePasswordDto dto);
    }
    public class AuthService : IAuthService
    {
        private readonly BlogDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(BlogDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
    /// <summary>
    /// Registers a new user and saves them to the database.
    /// </summary>
    /// <param name="userRegisterDto">The DTO containing user registration information.</param>
    /// <returns>Returns a UserResponseDto with the registered user's details.</returns>
    public async Task<UserResponseDto> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userRegisterDto.Username,
                Email = userRegisterDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password),
                IsPublic = userRegisterDto.IsPublic
            };
            if (await _context.Users.AnyAsync(u => u.Username == userRegisterDto.Username || u.Email == userRegisterDto.Email))
            {
                throw new InvalidOperationException("Username or email already exists.");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }
    /// <summary>
    /// Authenticates a user and returns a JWT token if credentials are valid.
    /// </summary>
    /// <param name="userLoginDto">The DTO containing user login credentials.</param>
    /// <returns>Returns a JWT token string if login is successful, otherwise null.</returns>
    public async Task<string> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == userLoginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
            {
                return null; // or throw an exception
            }

            return GenerateJwtToken(user);
        }
        
    /// <summary>
    /// Updates the privacy setting of a user account.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="isPublic">The new privacy status.</param>
    /// <returns>Returns true if the update was successful, false otherwise.</returns>
    public async Task<bool> UpdatePrivacyAsync(Guid userId, bool isPublic)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsPublic = isPublic;
            await _context.SaveChangesAsync();
            return true;
        }
    /// <summary>
    /// Resets the password for a user identified by email.
    /// </summary>
    /// <param name="dto">The DTO containing the email and new password.</param>
    /// <returns>Returns true if the password was reset successfully, false otherwise.</returns>
    public async Task<bool> ResetPasswordAsync(UpdatePasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return false;
            // Hash new password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <returns>Returns a JWT token string.</returns>
    private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}