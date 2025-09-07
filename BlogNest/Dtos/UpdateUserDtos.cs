namespace BlogNest.Dtos
{
    /// <summary>
    /// Data transfer object for updating user profile information.
    /// All properties are optional, allowing partial updates.
    /// </summary>
    public class UserUpdateDto
    {
        /// <summary>
        /// Gets or sets the new username. Null if not being updated.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the new email address. Null if not being updated.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets whether the user's profile should be public. Null if not being updated.
        /// </summary>
        public bool? IsPublic { get; set; }
    }
}
