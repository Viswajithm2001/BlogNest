
namespace BlogNest.Dtos
{
    /// <summary>
    /// Data transfer object for creating a new blog post.
    /// </summary>
    public class CreatePostDto
    {
        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the main content of the post.
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Gets or sets the list of tags associated with the post.
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the ID of the user creating the post.
        /// </summary>
        public Guid UserId { get; internal set; }
    }
}