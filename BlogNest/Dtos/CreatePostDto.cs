
namespace BlogNest.Dtos
{
    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
         public List<string> Tags { get; set; } = new List<string>();
        public Guid UserId { get; internal set; }
    }
}