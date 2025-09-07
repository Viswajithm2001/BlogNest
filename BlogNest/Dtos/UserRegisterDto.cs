namespace BlogNest.Dtos
{
    /// <summary>
    /// Data transfer object for new user registration requests.
    /// </summary>
    public class UserRegisterDto
    {
        /// <summary>
        /// Gets or sets the desired username for the new account.
        /// Must be unique across all users.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address for the new account.
        /// Must be unique across all users.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the new account.
        /// Should meet the application's security requirements.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets whether the user's profile should be public.
        /// Defaults to true for better discoverability.
        /// </summary>
        public bool IsPublic { get; set; } = true;
    }
}