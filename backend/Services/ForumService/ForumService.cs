using backend.DTOs;
using backend.Models;
using backend.Repositories.EventRepository;
using backend.Repositories.ForumRepository;
using backend.Services.EventService;

namespace backend.Services.ForumService
{
    public class ForumService : IForumService
    {
        private readonly IForumRepository _forumRepository;
        public ForumService(IForumRepository forumRepository)
        {
            _forumRepository = forumRepository;
        }
        public async Task<object> GetAllPost()
        {
            return await _forumRepository.GetAllPost();
        }
        public object GetPostById(int postId)
        {
            return _forumRepository.GetPostById(postId);
        }
        public object AddPost(Post post)
        {
            return _forumRepository.AddPost(post);
        }
        public dynamic GetPostByStatus(string? status, int accountId)
        {
            return _forumRepository.GetPostByStatus(status, accountId);
        }
        public object EditPost(EditPostDTO post)
        {
            return _forumRepository.EditPost(post);
        }
        public object DeletePost(int postId)
        {
            return _forumRepository.DeletePost(postId);
        }
        public object RejectPost(int postId)
        {
            return _forumRepository.RejectPost(postId);
        }
        public object SavePost(int postId, int accountId)
        {
            return _forumRepository.SavePost(postId, accountId);
        }
        public object UnsavePost(int postId, int accountId)
        {
            return _forumRepository.UnsavePost(postId, accountId);
        }
        public dynamic GetSavedPostByAccountId(int accountId)
        {
            return _forumRepository.GetSavedPostByAccountId(accountId);
        }
        public object ChangeStatusPost(int postId, string status)
        {
            return _forumRepository.ChangeStatusPost(postId, status);
        }
        public object LikePost(int postId, int accountId)
        {
            return _forumRepository.LikePost(postId, accountId);
        }
        public object UnlikePost(int postId, int accountId)
        {
            return _forumRepository.UnlikePost(postId, accountId);
        }
        public object CountLikedNumberByPost(int postId)
        {
            return _forumRepository.CountLikedNumberByPost(postId);
        }
        public object CountComment(int postId)
        {
            return _forumRepository.CountComment(postId);
        }
    }
}
