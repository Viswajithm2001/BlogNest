namespace BlogNest.Dtos
{
    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
    }
}