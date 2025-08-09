namespace BlogNest.Dtos
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public Guid PostId { get; set; }
    }
    public class CommentResponseDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string AuthorUsername { get; set; }
    }
    public class UpdateCommentDto
    {
        public string Content { get; set; }
    }
}