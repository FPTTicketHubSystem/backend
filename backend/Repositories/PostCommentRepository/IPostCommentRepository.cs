using backend.DTOs;
using backend.Models;

namespace backend.Repositories.PostCommentRepository
{
    public interface IPostCommentRepository
    {
        object AddPostcomment(Postcomment postcomment);
        dynamic GetCommentByPost(int postId);
        object ChangeStatusPostcomment(int postCommentId, string status);
        object EditComment(EditCommentDTO postcomment);
        object DeleteComment(int commentId);
    }
}
