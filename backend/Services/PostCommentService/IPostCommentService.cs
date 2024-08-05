using backend.DTOs;
using backend.Models;

namespace backend.Services.PostCommentService
{
    public interface IPostCommentService
    {
        object AddPostcomment(Postcomment postcomment);
        dynamic GetCommentByPost(int postId);
        object ChangeStatusPostcomment(int postCommentId, string status);
        object EditComment(EditCommentDTO postcomment);
        object DeleteComment(int commentId);
    }
}
