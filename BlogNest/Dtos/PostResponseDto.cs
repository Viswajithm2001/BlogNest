namespace BlogNest.Dtos
{
    public class PostResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public string AuthorUsername { get; set; }
        public bool IsAuthorPublic { get; set; } 
    }
}