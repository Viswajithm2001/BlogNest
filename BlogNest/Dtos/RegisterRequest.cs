namespace BlogNest.Dtos
{
    /// <summary>
    /// Data transfer object for user registration requests.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Gets or sets the desired username for the new account.
        /// The username must be unique across all users.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address for the new account.
        /// The email must be unique across all users.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for the new account.
        /// The password should meet the application's security requirements.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}