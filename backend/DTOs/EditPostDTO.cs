namespace backend.DTOs
{
    public class EditPostDTO
    {
        public int PostId { get; set; }
        public string? PostText { get; set; }
        public string? PostFile { get; set; }
        public string? Status { get; set; }
    }
}
