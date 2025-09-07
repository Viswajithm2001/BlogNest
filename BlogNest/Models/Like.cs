namespace BlogNest.Models
{
    /// <summary>
    /// Represents a user's like on a blog post.
    /// </summary>
    public class Like
    {
        /// <summary>
        /// Gets or sets the unique identifier for the like.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the like.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the post that was liked.
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// Gets or sets the user who created the like.
        /// Navigation property for Entity Framework.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the post that was liked.
        /// Navigation property for Entity Framework.
        /// </summary>
        public Post Post { get; set; }
    }
}