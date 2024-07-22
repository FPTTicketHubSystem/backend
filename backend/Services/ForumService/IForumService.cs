using backend.DTOs;
using backend.Models;

namespace backend.Services.ForumService
{
    public interface IForumService
    {
        Task<object> GetAllPost();
        object AddPost(PostDTO postDTO);
        object EditPost(int eventId, PostDTO updatedEventDto);
        object DeletePost(int eventId);

        object AddCommentPost(int postid, PostCommentDTO postCommentDTO);

        Task<object> getcomment(int postid);
        //Task<object> FindPost();
        Task<object> likepost(int postId, int iduser);
        object EditComment(int commentId, String Content);
        object DeleteComment(int commentId);
        Task<object> GetAllPostSaved(int userId);
    }
}
