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
        public object AddPost(PostDTO postDTO)
        {
            var result = _forumRepository.AddPost(postDTO);
            return result;
        }

        public object DeletePost(int eventId)
        {
            var result = _forumRepository.DeletePost(eventId);
            return result;
        }

        public object EditPost(int PostId, PostDTO postDTO)
        {
            var result = _forumRepository.EditPost(PostId, postDTO);
            return result;
        }

        public Task<object> GetAllPostSaved(int iduser)
        {
            return _forumRepository.GetAllPostSaved(iduser);
        }

        public async Task<object> GetAllPost()
        {
            return await _forumRepository.GetAllPost();
        }

        public object AddCommentPost(int postId, PostCommentDTO postCommentDTO)
        {
            var result = _forumRepository.AddCommentPost(postId, postCommentDTO);
            return result;
        }
        public async Task<object> getcomment(int postid)
        {
            return await _forumRepository.getcomment(postid);
        }

        public async Task<object> likepost(int postId, int iduser)
        {
            return await _forumRepository.likepost(postId, iduser);
        }
        public object DeleteComment(int commentId)
        {
            var result = _forumRepository.DeleteComment(commentId);
            return result;
        }

        public object EditComment(int commentId, String Content)
        {
            var result = _forumRepository.EditComment(commentId, Content);
            return result;
        }
    }
}
