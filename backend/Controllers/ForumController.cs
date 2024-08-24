using backend.DTOs;
using backend.Models;
using backend.Repositories.ForumRepository;
using backend.Services.ForumService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace backend.Controllers
{
    [Route("api/forum")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly IForumService _postService;
        private readonly IConfiguration _configuration;
        public ForumController(IForumService forumService, IConfiguration configuration)
        {
            _postService = forumService;
            _configuration = configuration;
        }

        [HttpGet("getAllPost")]
        public async Task<ActionResult> GetAllPost()
        {
            try
            {
                var data = await _postService.GetAllPost();

                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("getPostDetail")]
        public async Task<ActionResult> GetPostById(int postId)
        {
            try
            {
                var result = _postService.GetPostById(postId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("AddPost")]
        public async Task<ActionResult> AddPost([FromBody] ForumDTO addPost)
        {
            try
            {
                var post = new Post();
                post.AccountId = addPost.AccountId;
                post.PostText = addPost.PostText;
                post.PostFile = addPost.PostFile;
                post.Status = "Chờ duyệt";
                post.CreateDate = DateTime.Now;
                var result = _postService.AddPost(post);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("ChangeStatusPost")]
        public async Task<ActionResult> ChangeStatusPost(int postId, string status)
        {
            try
            {
                var result = _postService.ChangeStatusPost(postId, status);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("EditPost")]
        public async Task<ActionResult> EditPost(EditPostDTO post)
        {
            try
            {
                var result = _postService.EditPost(post);
                return Ok(result);
            }
            catch { return BadRequest(); }
        }

        [HttpGet("GetPostByStatus")]
        public ActionResult GetPostByStatus(string? status, int accountId)
        {
            var result = _postService.GetPostByStatus(status, accountId);
            return Ok(result);
        }

        [HttpGet("CountCommentByPost")]
        public async Task<ActionResult> CountComment(int postId)
        {
            try
            {
                var result = _postService.CountComment(postId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("CountLikedNumberByPost")]
        public async Task<ActionResult> CountLikedNumberByPost(int postId)
        {
            try
            {
                var result = _postService.CountLikedNumberByPost(postId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("LikePost")]
        public async Task<ActionResult> LikePost(int postId, int accountId)
        {
            try
            {
                var result = _postService.LikePost(postId, accountId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("UnlikePost")]
        public async Task<IActionResult> UnlikePost(int postId, int accountId)
        {
            try
            {
                var result = _postService.UnlikePost(postId, accountId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("DeletePost")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                var result = _postService.DeletePost(postId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("RejectPost")]
        public async Task<IActionResult> RejectPost(int postId)
        {
            try
            {
                var result = _postService.RejectPost(postId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("SavePost")]
        public async Task<ActionResult> SavePost(int postId, int accountId)
        {
            try
            {
                var result = _postService.SavePost(postId, accountId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("UnSavePost")]
        public async Task<IActionResult> UnsavePost(int postId, int accountId)
        {
            try
            {
                var result = _postService.UnsavePost(postId, accountId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetSavedPostByAccount")]
        public ActionResult GetSavedPostByAccount(int accountId)
        {
            var posts = _postService.GetSavedPostByAccountId(accountId);
            return Ok(posts);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("getAllPostAdmin")]
        public async Task<ActionResult> GetAllPostAdmin()
        {
            try
            {
                var data = await _postService.GetAllPostAdmin();
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
