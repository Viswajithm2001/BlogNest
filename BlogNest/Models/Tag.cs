namespace BlogNest.Models
{
    /// <summary>
    /// Represents a tag that can be applied to blog posts for categorization.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tag.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of posts associated with this tag.
        /// Navigation property for Entity Framework.
        /// </summary>
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}