using backend.DTOs;
using backend.Models;
using backend.Repositories.PostCommentRepository;

namespace backend.Services.PostCommentService
{
    public class PostCommentService : IPostCommentService
    {
        private readonly IPostCommentRepository _postcommentRepository;
        public PostCommentService(IPostCommentRepository postCommentRepository)
        {
            _postcommentRepository = postCommentRepository;
        }
        public object AddPostcomment(Postcomment postcomment)
        {
            var result = _postcommentRepository.AddPostcomment(postcomment);
            return result;
        }

        public dynamic GetCommentByPost(int postId)
        {
            return _postcommentRepository.GetCommentByPost(postId);
        }

        public object ChangeStatusPostcomment(int postcommentId, string status)
        {
            return _postcommentRepository.ChangeStatusPostcomment(postcommentId, status);
        }

        public object EditComment(EditCommentDTO postcomment)
        {
            return _postcommentRepository.EditComment(postcomment);
        }
        public object DeleteComment(int commentId)
        {
            return _postcommentRepository.DeleteComment(commentId);
        }
    }
}
