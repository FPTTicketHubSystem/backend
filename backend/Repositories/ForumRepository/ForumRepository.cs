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
        public async Task<object> GetAllPost()
        {
            var data = _context.Posts
                .Include(p => p.Account)
                .Include(p => p.Postcomments)
                .Where(p => p.Status == "Đã duyệt")
                .OrderByDescending(p => p.CreateDate)
                .Select(p =>
              new
              {
                  p.PostId,
                  p.AccountId,
                  p.Account.Avatar,
                  p.Account.FullName,
                  p.PostText,
                  p.PostFile,
                  p.Status,
                  p.CreateDate,
                  p.Postlikes,
                  p.Postfavorites,
                  countComment = p.Postcomments.Count(),
                  countLike = p.Postlikes.Count(),

              });
            return data;
        }
        public object GetPostById(int postId)
        {
            var data = _context.Posts.SingleOrDefault(x => x.PostId == postId);
            if (data == null)
            {
                return null;
            }
            return data;
        }
        public object AddPost(Post post)
        {
            try
            {
                _context.Add(post);
                _context.SaveChanges();
                return new
                {
                    message = "Add post successfully",
                    post,
                    status = 200
                };
            }
            catch
            {
                return new
                {
                    message = "Add post fail",
                    status = 400
                };
            }
        }
        public dynamic GetPostByStatus(string? status, int accountId)
        {
            var checkAccount = _context.Accounts.SingleOrDefault(a => a.AccountId == accountId);
            if (checkAccount.RoleId == 1)
            {
                var posts = _context.Posts
                .Include(p => p.Postcomments)
                .Include(p => p.Postlikes)
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.CreateDate)
                .Select(p =>
             new
             {
                 p.PostId,
                 p.Account.FullName,
                 p.Account.Avatar,
                 p.PostText,
                 p.PostFile,
                 p.Status,
                 p.CreateDate,
                 p.Postlikes,
                 p.Postfavorites,
                 countComment = p.Postcomments.Count(),
                 countLike = p.Postlikes.Count()
             });
                return posts;
            }
            else
            {
                var posts = _context.Posts
                .Include(p => p.Postcomments)
                .Include(p => p.Postlikes)
                .Where(p => p.Status == status && p.AccountId == accountId)
                .OrderByDescending(p => p.CreateDate)
                .Select(p =>
                    new
                    {
                        p.PostId,
                        p.Account.FullName,
                        p.Account.Avatar,
                        p.PostText,
                        p.PostFile,
                        p.Status,
                        p.CreateDate,
                        p.Postlikes,
                        p.Postfavorites,
                        countComment = p.Postcomments.Count(),
                        countLike = p.Postlikes.Count()
                    }); ;
                return posts;
            }
        }
        public object EditPost(EditPostDTO post)
        {
            try
            {
                var editPost = _context.Posts.SingleOrDefault(x => x.PostId == post.PostId);
                if (editPost == null)
                {
                    return new
                    {
                        message = "Post Not Found",
                        status = 200,
                    };
                }
                editPost.PostText = post.PostText;
                editPost.PostFile = post.PostFile;
                editPost.Status = "Đã duyệt";
                _context.SaveChanges();
                return new
                {
                    message = "Post edited successfully",
                    status = 200,
                    editPost,
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
        public object DeletePost(int postId)
        {
            var post = _context.Posts.SingleOrDefault(x => x.PostId == postId);
            if (post == null)
            {
                return new
                {
                    message = "The post doesn't exist in database",
                    status = 400
                };
            }
            else
            {
                post.Status = "Đã xóa";
                _context.SaveChanges();
                return new
                {
                    status = 200,
                    message = "Post deleted successfully!"
                };
            }
        }
        public object RejectPost(int postId)
        {
            var post = _context.Posts.SingleOrDefault(x => x.PostId == postId);
            if (post == null || post.Status != "Chờ duyệt")
            {
                return new
                {
                    message = "The post doesn't exist in database or has been approved",
                    status = 400
                };
            }
            else
            {
                post.Status = "Từ chối";
                _context.SaveChanges();
                return new
                {
                    status = 200,
                    message = "The post has been rejected to upload!"
                };
            }
        }
        public object SavePost(int postId, int accountId)
        {
            var post = _context.Posts.SingleOrDefault(x => x.PostId == postId);
            if (post == null)
            {
                return new
                {
                    message = "The post doesn't exist in the database",
                    status = 400
                };
            }
            else
            {
                var postSave = new Postfavorite
                {
                    AccountId = accountId,
                    PostId = postId,
                    Status = "Đã lưu"
                };
                var checkExist = _context.Postfavorites.Any(c => c.PostId == postId && c.AccountId == accountId);
                {
                    if (checkExist)
                    {
                        return new
                        {
                            message = "This account has saved this post before!"
                        };
                    }
                    else
                    {
                        _context.Postfavorites.Add(postSave);
                        _context.SaveChanges();

                        return new
                        {
                            status = 200,
                            postSave,
                            message = "Post saved"
                        };
                    }
                }
            }
        }
        public object UnsavePost(int postId, int accountId)
        {
            var postUnsave = _context.Postfavorites.SingleOrDefault(x => x.PostId == postId && x.AccountId == accountId);
            if (postUnsave == null)
            {
                return new
                {
                    message = "This account has not saved this post before!",
                    status = 400
                };
            }
            else
            {
                _context.Postfavorites.Remove(postUnsave);
                _context.SaveChanges();
                return new
                {
                    status = 200,
                    message = "Unsave post successfully!"
                };
            }
        }
        public dynamic GetSavedPostByAccountId(int accountId)
        {
            try
            {
                var posts = _context.Postfavorites
                    .Include(p => p.Post)
                    .Where(p => p.AccountId == accountId && p.Status == "Đã lưu" && p.Post.Status == "Đã duyệt")
                    .OrderByDescending(p => p.Post.CreateDate)
                    .Select(p =>
                new
                {
                    p.PostId,
                    p.AccountId,
                    p.Account.FullName,
                    p.Account.Avatar,
                    p.Post.PostText,
                    p.Post.PostFile,
                    p.Post.Status,
                    p.Post.CreateDate,
                    p.Post.Postlikes,
                    p.Post.Postfavorites,
                    countComment = p.Post.Postcomments.Count(),
                    countLike = p.Post.Postlikes.Count()
                });
                return posts;
            }
            catch (Exception ex)
            {
                return new
                {
                    message = "This account has not saved any post before!"
                };
            }
        }
        public object ChangeStatusPost(int postId, string status)
        {
            var updateStatus = _context.Posts.SingleOrDefault(x => x.PostId == postId);
            if (updateStatus == null)
            {
                return new
                {
                    message = "The post doesn't exist in database",
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
                    message = "Post Update successfully!"
                };
            }
        }
        public object LikePost(int postId, int accountId)
        {
            var post = _context.Posts.SingleOrDefault(x => x.PostId == postId);
            if (post == null)
            {
                return new
                {
                    message = "The post doesn't exist in the database",
                    status = 400
                };
            }
            else
            {
                var postlike = new Postlike
                {
                    AccountId = accountId,
                    PostId = postId,
                    LikeDate = DateTime.Now
                };
                var checkExist = _context.Postlikes.Any(c => c.PostId == postId && c.AccountId == accountId);
                {
                    if (checkExist)
                    {
                        return new
                        {
                            message = "This account has liked this post before!"
                        };
                    }
                    else
                    {
                        _context.Postlikes.Add(postlike);
                        _context.SaveChanges();

                        return new
                        {
                            status = 200,
                            postlike,
                            message = "Post liked"
                        };
                    }
                }
            }
        }
        public object UnlikePost(int postId, int accountId)
        {
            var postUnlike = _context.Postlikes.SingleOrDefault(x => x.PostId == postId && x.AccountId == accountId);
            if (postUnlike == null)
            {
                return new
                {
                    message = "This account has not liked this post before!",
                    status = 400
                };
            }
            else
            {
                _context.Postlikes.Remove(postUnlike);
                _context.SaveChanges();
                return new
                {
                    status = 200,
                    message = "Unlike post successfully!"
                };
            }
        }
        public object CountLikedNumberByPost(int postId)
        {
            try
            {
                var post = _context.Posts.SingleOrDefault(x => x.PostId == postId);
                var countLiked = _context.Postlikes.Where(x => x.PostId == postId).Count();
                if (post == null)
                {
                    return new
                    {
                        message = "Cannot find this post",
                        status = 200,
                    };
                }
                return new
                {
                    status = 200,
                    countLiked,
                };
            }
            catch
            {
                return new
                {
                    status = 400,
                };
            }
        }
        public object CountComment(int postId)
        {
            try
            {
                var post = _context.Posts.SingleOrDefault(x => x.PostId == postId);
                var countComment = _context.Postcomments.Where(x => x.PostId == postId).Count();
                if (post == null)
                {
                    return new
                    {
                        message = "Cannot find this post",
                        status = 200,
                    };
                }
                return new
                {
                    countComment,
                };
            }
            catch
            {
                return new
                {
                    status = 400,
                };
            }
        }
        public async Task<object> GetAllPostAdmin()
        {
            var data = await _context.Posts
                .Include(p => p.Account)
                .Include(p => p.Postcomments)
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new
                {
                    p.PostId,
                    p.AccountId,
                    p.Account.Avatar,
                    p.Account.FullName,
                    p.PostText,
                    p.PostFile,
                    p.Status,
                    p.CreateDate,
                    p.Postlikes,
                    p.Postfavorites,
                    countComment = p.Postcomments.Count(),
                    countLike = p.Postlikes.Count(),
                })
                .ToListAsync();

            return data;
        }
    }
}