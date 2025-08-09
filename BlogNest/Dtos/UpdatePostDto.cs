namespace BlogNest.Dtos
{
    public class UpdatePostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}