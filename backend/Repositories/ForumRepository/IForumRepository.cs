using backend.DTOs;
using backend.Models;

namespace backend.Repositories.ForumRepository
{
    public interface IForumRepository
    {
        Task<object> GetAllPost();
        object GetPostById(int postId);
        object AddPost(Post post);
        dynamic GetPostByStatus(string? status, int accountId);
        object EditPost(EditPostDTO post);
        object DeletePost(int postId);
        object RejectPost(int postId);
        object SavePost(int postId, int accountId);
        object UnsavePost(int postId, int accountId);
        dynamic GetSavedPostByAccountId(int accountId);
        object ChangeStatusPost(int postId, string status);
        object LikePost(int postId, int accountId);
        object UnlikePost(int postId, int accountId);
        object CountLikedNumberByPost(int postId);
        object CountComment(int postId);
    }
}
