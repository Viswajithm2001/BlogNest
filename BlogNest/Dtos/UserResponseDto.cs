namespace BlogNest.Dtos
{
    /// <summary>
    /// Data transfer object for returning user information in API responses.
    /// Contains only safe, public user information.
    /// </summary>
    public class UserResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public required string Email { get; set; }
    }
}