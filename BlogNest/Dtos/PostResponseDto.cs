namespace BlogNest.Dtos
{
    /// <summary>
    /// Data transfer object for returning blog post information in API responses.
    /// </summary>
    public class PostResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the post.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the blog post.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the main content of the blog post.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the post was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who authored the post.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the username of the post author.
        /// </summary>
        public required string AuthorUsername { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the author's profile is public.
        /// </summary>
        public bool IsAuthorPublic { get; set; }

        /// <summary>
        /// Gets or sets the list of tags associated with the post.
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the collection of comments on this post.
        /// </summary>
        public ICollection<CommentResponseDto> Comments { get; set; } = new List<CommentResponseDto>();

        /// <summary>
        /// Gets or sets the URL of the post's featured image, if any.
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}