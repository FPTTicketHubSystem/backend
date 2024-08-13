using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.PostCommentRepository
{
    public class PostCommentRepository : IPostCommentRepository
    {
        private readonly FpttickethubContext _context;
        public PostCommentRepository(FpttickethubContext context)
        {
            _context = context; 
        }
        public dynamic GetCommentByPost(int postId)
        {
            var postcomments = _context.Postcomments
                .Include(p => p.Post)
                .Include(p => p.Account)
                .Where(p => p.PostId == postId && p.Status == "Uploaded")
                .OrderByDescending(p => p.CommentDate)
                .Select(p =>
            new
            {
                p.PostCommentId,
                p.AccountId,
                p.Account.FullName,
                p.Account.Avatar,
                p.PostId,
                p.Content,
                p.FileComment,
                p.Status,
                p.CommentDate
            });
            return postcomments;
        }
        public object AddPostcomment(Postcomment postcomment)
        {
            try
            {
                _context.Add(postcomment);
                _context.SaveChanges();
                return new
                {
                    message = "Comment successfully",
                    postcomment,
                    status = 200
                };
            }
            catch
            {
                return new
                {
                    message = "Cannot comment in this post",
                    status = 400
                };
            }
        }
        public object EditComment(EditCommentDTO postcomment)
        {
            try
            {
                var editComment = _context.Postcomments.SingleOrDefault(x => x.PostCommentId == postcomment.PostCommentId);
                if (editComment == null)
                {
                    return new
                    {
                        message = "Comment Not Found",
                        status = 200,
                    };
                }

                editComment.Content = postcomment.Content;
                editComment.FileComment = postcomment.FileComment;
                editComment.Status = "Uploaded";
                _context.SaveChanges();
                return new
                {
                    message = "Comment edited successfully",
                    status = 200,
                    editComment,
                };
            }
            catch
            {
                return new
                {
                    message = "Comment edited failed",
                    status = 400,
                };
            }
        }
        public object DeleteComment(int commentId)
        {
            var comment = _context.Postcomments.SingleOrDefault(x => x.PostCommentId == commentId);
            if (comment == null)
            {
                return new
                {
                    message = "Comment does not exist!",
                    status = 400
                };
            }
            else
            {
                comment.Status = "Deleted";
                _context.SaveChanges();
                return new
                {
                    status = 200,
                    message = "Comment deleted successfully!"
                };
            }
        }
        public object ChangeStatusPostcomment(int postcommentId, string status)
        {
            var updateStatus = _context.Postcomments.SingleOrDefault(x => x.PostCommentId == postcommentId);
            if (updateStatus == null)
            {
                return new
                {
                    message = "The comment doesn't exist in database",
                    status = 400
                };
            }
            else
            {
                updateStatus.Status = status;
                _context.SaveChanges();
                return new
                {
                    status = 200,
                    updateStatus,
                    message = "Update successfully!"
                };
            }
        }
    }
}
