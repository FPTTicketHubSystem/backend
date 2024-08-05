namespace backend.DTOs
{
    public class EditCommentDTO
    {
        public int PostCommentId { get; set; }
        public string? Content { get; set; }
        public string? FileComment { get; set; }
        public string? Status { get; set; }
    }
}
