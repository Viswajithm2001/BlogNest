using System;
using System.ComponentModel.DataAnnotations;

namespace BlogNest.Models
{
    /// <summary>
    /// Represents a user in the blog system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the username for the user.
        /// Must be unique across all users.
        /// </summary>
        [Required]
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address for the user.
        /// Must be unique and in valid email format.
        /// </summary>
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the hashed password for the user.
        /// Raw passwords should never be stored.
        /// </summary>
        [Required]
        public required string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets whether the user's profile is publicly visible.
        /// Defaults to true for better discoverability.
        /// </summary>
        public bool IsPublic { get; set; } = true;

        /// <summary>
        /// Gets or sets the collection of posts created by this user.
        /// Navigation property for Entity Framework.
        /// </summary>
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        /// <summary>
        /// Gets or sets the collection of comments made by this user.
        /// Navigation property for Entity Framework.
        /// </summary>
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// Gets or sets the collection of likes made by this user.
        /// Navigation property for Entity Framework.
        /// </summary>
        public ICollection<Like> Likes { get; set; } = new List<Like>();

        /// <summary>
        /// Gets or sets the URL of the user's profile picture, if any.
        /// </summary>
        public string? ProfilePictureUrl { get; internal set; }
    }
}