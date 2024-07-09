namespace backend.DTOs
{
    public class NewsDTO
    {
        public int NewsId { get; set; }

        public int AccountId { get; set; }

        public string CoverImage { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public string Status { get; set; }
    }
}
