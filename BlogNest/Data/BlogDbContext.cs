using BlogNest.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogNest.Data
{
    /// <summary>
    /// Database context for the BlogNest application, managing entities such as Users, Posts, Comments, Likes, and Tags.
    /// </summary>
    public class BlogDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the BlogDbContext.
        /// </summary>
        /// <param name="options">The options for configuring the context.</param>
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Users table in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the Posts table in the database.
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// Gets or sets the Comments table in the database.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the Likes table in the database.
        /// </summary>
        public DbSet<Like> Likes { get; set; }

        /// <summary>
        /// Gets or sets the Tags table in the database.
        /// </summary>
        public DbSet<Tag> Tags { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Posts);
                base.OnModelCreating(modelBuilder);
        }
    }

}