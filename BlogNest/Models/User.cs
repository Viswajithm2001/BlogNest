using System;
using System.ComponentModel.DataAnnotations;

namespace BlogNest.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public bool IsPublic { get; set; } = true;
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public string? ProfilePictureUrl { get; internal set; }
    }
}