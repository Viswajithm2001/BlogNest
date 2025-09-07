namespace BlogNest.Dtos
{
    /// <summary>
    /// Data transfer object for creating a new comment on a post.
    /// </summary>
    public class CreateCommentDto
    {
        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Gets or sets the ID of the post this comment belongs to.
        /// </summary>
        public Guid PostId { get; set; }
    }

    /// <summary>
    /// Data transfer object for returning comment information in API responses.
    /// </summary>
    public class CommentResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the comment.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the comment was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the ID of the post this comment belongs to.
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who created the comment.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the username of the comment author.
        /// </summary>
        public required string AuthorUsername { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating an existing comment.
    /// </summary>
    public class UpdateCommentDto
    {
        /// <summary>
        /// Gets or sets the new content of the comment.
        /// </summary>
        public required string Content { get; set; }
    }
}