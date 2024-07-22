using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Diagnostics;

namespace backend.Repositories.ForumRepository
{
    public class ForumRepository : IForumRepository
    {
        private readonly FpttickethubContext _context;
        public ForumRepository(FpttickethubContext context)
        {
            _context = context;
        }

        public object AddPost(PostDTO postDTO)
        {
            try
            {
                var newPost = new Post
                {
                    AccountId = postDTO.AccountId, //cái ni lấy trong user.AccountId UserContext
                    PostFile = postDTO.PostFile, //cái ni up cái ảnh firebase vô 
                    PostText = postDTO.PostText, //ni nhập ô text
                    CreateDate = DateTime.Now, //thì chưa làm lại frontend mà, làm một state sẽ chứa object đi xuống đi, rồi lấy mấy cái như t cmt ở trên đó làm thử xem với chứ giwof mò tới sang

                    Status = "Chờ duyệt",

                };

                _context.Posts.Add(newPost);
                _context.SaveChanges();

                return new
                {
                    message = "Event Added",
                    status = 200,
                    newPost
                };
            }
            catch
            {
                return new
                {
                    message = "Add Event Fail",
                    status = 400
                };
            }
        }

        public object DeletePost(int postId)
        {
            try
            {
                // Find the post in the database
                var postToDelete = _context.Posts.Find(postId);

                if (postToDelete == null)
                {
                    return new
                    {
                        message = "Post not found",
                        status = 404
                    };
                }

                // Delete related comments first
                var relatedComments = _context.Postcomments.Where(pc => pc.PostId == postId);
                _context.Postcomments.RemoveRange(relatedComments);

                var relatedFavorites = _context.Postfavorites.Where(pf => pf.PostId == postId);
                _context.Postfavorites.RemoveRange(relatedFavorites);

                // Remove the post from the context
                _context.Posts.Remove(postToDelete);
                _context.SaveChanges();

                return new
                {
                    message = "Post deleted successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = $"Delete post failed: {ex.Message}",
                    status = 400
                };
            }
        }




        public object EditPost(int postId, PostDTO updatedPostDto)
        {
            try
            {
                // Find the post in the database
                var postToUpdate = _context.Posts.Find(postId);

                if (postToUpdate == null)
                {
                    return new
                    {
                        message = "Post not found",
                        status = 404
                    };
                }

                //Update properties with new values
                postToUpdate.PostFile = updatedPostDto.PostFile;
                postToUpdate.PostText = updatedPostDto.PostText;
                postToUpdate.Status = updatedPostDto.Status;

                //Save changes to the database
                _context.SaveChanges();

                return new
                {
                    message = "Post updated successfully",
                    status = 200,
                    updatedPost = postToUpdate
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = $"Update post failed: {ex.Message}",
                    status = 400
                };
            }
        }

        //public Task<object> FindPost()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<object> GetAllPost()
        {
            var data = _context.Posts
                 .Include(e => e.Account)
                 .Include(e => e.Postcomments)
                 .Include(e => e.Postlikes)
                 .OrderByDescending(e => e.CreateDate)
                 .Select(e =>
                 new
                 {
                     e.PostId,
                     e.Account.AccountId,
                     e.PostFile,
                     e.PostText,
                     e.Account.FullName,
                     e.Account.Avatar,
                     e.CreateDate,
                     e.Status,
                     e.Postcomments.Count,
                 });
            return data;
        }

        public object AddCommentPost(int postId, PostCommentDTO postCommentDTO)
        {
            try
            {
                // Find the post in the database
                var post = _context.Posts.Find(postId);

                if (post == null)
                {
                    return new
                    {
                        message = "Post not found",
                        status = 404
                    };
                }

                var newComment = new Postcomment()
                {
                    AccountId = postCommentDTO.AccountId,
                    PostId = postId, // Assign postId directly from method parameter
                    Content = postCommentDTO.Content,
                    CommentDate = DateTime.Now,

                };

                // Initialize Postcomments collection if null
                if (post.Postcomments == null)
                {
                    post.Postcomments = new List<Postcomment>();
                }

                // Add the new comment to the post's comments collection
                post.Postcomments.Add(newComment);

                _context.Posts.Update(post);
                _context.SaveChanges();

                return new
                {
                    message = $"Comment added successfully!",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = $"Error adding comment: {ex.Message}",
                    status = 500 // or another appropriate error status code
                };
            }
        }

        public async Task<object> getcomment(int postId)
        {
            var post = _context.Postcomments.Where(pf => pf.PostId == postId);

            return post;
          }
  

        public async Task<object> likepost(int postId, int iduser)
        {
            /*ICollection < Postfavorite > pv = new List<Postfavorite>();*/
            var post = _context.Posts.Find(postId);
            var favpost = _context.Postfavorites
            .FirstOrDefault(pf => pf.PostId == postId && pf.AccountId == iduser);
            if (favpost != null)
            {
                if (favpost.Status == "like")
                {
                    favpost.Status = "unlike";
                }
                else
                {
                    favpost.Status = "like";
                }
                _context.Postfavorites.Update(favpost);
            }
            else
            {
                Postfavorite like = new Postfavorite();
                like.Status = "like";
                like.PostId = postId;
                like.AccountId = iduser;
                _context.Postfavorites.Add(like);
            }
            _context.SaveChanges();


            return new
            {
                message = $"Like added successfully!",
                status = 200
            };
        }

        public object DeleteComment(int commentId)
        {
            try
            {
                // Find the post in the database
                var deleteComment = _context.Postcomments.Find(commentId);

                if (deleteComment == null)
                {
                    return new
                    {
                        message = "Post not found",
                        status = 404
                    };
                }

                // Remove the post from the context
                _context.Postcomments.Remove(deleteComment);
                _context.SaveChanges();

                return new
                {
                    message = "Post deleted successfully",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = $"Delete post failed: {ex.Message}",
                    status = 400
                };
            }
        }



        public object EditComment(int commentId, String Content)
        {
            try
            {
                // Find the post in the database
                var postToUpdate = _context.Postcomments.Find(commentId);

                if (postToUpdate == null)
                {
                    return new
                    {
                        message = "Post not found",
                        status = 404
                    };
                }
                postToUpdate.Content = Content;

                //Save changes to the database
                _context.SaveChanges();

                return new
                {
                    message = "Post updated successfully",
                    status = 200,
                    updatedPost = postToUpdate
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = $"Update post failed: {ex.Message}",
                    status = 400
                };
            }
        }

        public async Task<object> GetAllPostSaved(int userId)
        {
            var postfv = _context.Postfavorites
             .Where(pf => pf.AccountId == userId && pf.Status == "like")
             .ToList();
            List<Post> listpost = new List<Post>();
            foreach (var post in postfv)
            {
                Post p = new Post();
                p = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == post.PostId);
                listpost.Add(p);
            }
            return postfv;
        }
    }
}
