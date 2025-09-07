namespace BlogNest.Models
{
    /// <summary>
    /// Represents a blog post in the system.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Gets or sets the unique identifier for the post.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the blog post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the main content of the blog post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the post was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the post.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who created the post.
        /// Navigation property for Entity Framework.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the collection of comments on this post.
        /// Navigation property for Entity Framework.
        /// </summary>
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// Gets or sets the collection of likes on this post.
        /// Navigation property for Entity Framework.
        /// </summary>
        public ICollection<Like> Likes { get; set; } = new List<Like>();

        /// <summary>
        /// Gets or sets the collection of tags associated with this post.
        /// Navigation property for Entity Framework.
        /// </summary>
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        /// <summary>
        /// Gets or sets the URL of the post's featured image, if any.
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}