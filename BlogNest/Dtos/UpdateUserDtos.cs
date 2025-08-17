namespace BlogNest.Dtos
{
    public class UserUpdateDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool? IsPublic { get; set; }
    }
}
