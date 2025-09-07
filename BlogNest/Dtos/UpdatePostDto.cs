namespace BlogNest.Dtos
{
    /// <summary>
    /// Data transfer object for updating an existing blog post.
    /// </summary>
    public class UpdatePostDto
    {
        /// <summary>
        /// Gets or sets the updated title of the blog post.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the updated content of the blog post.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Gets or sets the updated list of tags associated with the post.
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();
    }
}