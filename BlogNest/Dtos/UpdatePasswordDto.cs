namespace BlogNest.Dtos
{
    /// <summary>
    /// Data transfer object for updating a user's password.
    /// </summary>
    public class UpdatePasswordDto
    {
        /// <summary>
        /// Gets or sets the new password for the user.
        /// </summary>
        public required string NewPassword { get; set; }
        /// <summary>
        /// Gets or sets the confirm password for the user
        /// </summary>
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email { get; set; }
    }
}