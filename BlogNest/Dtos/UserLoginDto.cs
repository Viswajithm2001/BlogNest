namespace BlogNest.Dtos
{
    /// <summary>
    /// Data transfer object for user login requests.
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// Gets or sets the username for authentication.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for authentication.
        /// </summary>
        public required string Password { get; set; }
    }
}