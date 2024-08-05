using backend.Models;

namespace backend.DTOs
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public int AccountId { get; set; }
        public string PostText { get; set; }
        public string PostFile { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }

        public List<PostCommentDTO> comment { get; set; }

    }
    public class PostCommentDTO
    {
        public int PostCommentId { get; set; }
        public int AccountId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public string FileComment { get; set; }
        public DateTime CommentDate { get; set; }
        public string Status { get; set; }
    }
    public class PostFavoriteDTO
    {
        public int PostFavoriteId { get; set; }
        public int AccountId { get; set; }
        public int PostId { get; set; }
        public string Status { get; set; }

    }

/*    public class PostLikeDTO
    {
        public int PostLikeId { get; set; }
        public int AccountId { get; set; }
        public int PostId { get; set; }
        public DateTime LikeDate { get; set; }
        public string Status { get; set; }
    }*/

}
