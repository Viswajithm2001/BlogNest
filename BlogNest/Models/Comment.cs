namespace BlogNest.Models
{
    /// <summary>
    /// Represents a user comment on a blog post in the system.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Gets or sets the unique identifier for the comment.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the text content of the comment.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the comment was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the post this comment belongs to.
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// Gets or sets the associated blog post.
        /// Navigation property for Entity Framework.
        /// </summary>
        public Post Post { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the comment.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who created the comment.
        /// Navigation property for Entity Framework.
        /// </summary>
        public User User { get; set; }
    }
}