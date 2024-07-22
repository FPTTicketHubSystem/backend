using backend.DTOs;
using backend.Models;
using backend.Repositories.ForumRepository;
using backend.Services.ForumService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace backend.Controllers
{
    [Route("api/forum")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly IForumService _forumService;

        public ForumController(IForumService forumService)
        {
            _forumService = forumService;
        }
        //GET: api/post
        [HttpGet("GetAllPost")]
        public async Task<ActionResult> GetAllPost()
        {
            try
            {
                var data = await _forumService.GetAllPost();
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }
        // POST: api/post
        [HttpPost("addPost")]
        public async Task<ActionResult> AddPost(PostDTO postDTO)
        {
            try
            {
                postDTO.comment = null;
                var result = _forumService.AddPost(postDTO);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        //// POST: api/post
        [HttpPost("deletePost")]
        public async Task<ActionResult> DeletePost(int idPost)
        {
            try
            {
                var result = _forumService.DeletePost(idPost);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: api/post
        [HttpPost("editPost")]
        public async Task<ActionResult> EditPost(int idPost, PostDTO editpost)
        {
            try
            {
                Debug.WriteLine(idPost);
                var result = _forumService.EditPost(idPost, editpost);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: api/post
        [HttpPost("addcommentPost")]
        public async Task<ActionResult> AddComentPost(int postId, PostCommentDTO postDTO)
        {
            try
            {
                var result = _forumService.AddCommentPost(postId, postDTO);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("getCommentByPost")]
        public async Task<ActionResult> getCommentByPost(int postId)
        {
            try
            {
                var result = _forumService.getcomment(postId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("actionlikepost")]
        public async Task<ActionResult> actionlikepost(int postId, int userId)
        {
            try
            {
                var result = _forumService.likepost(postId, userId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("deleteComment")]
        public async Task<ActionResult> deleteComment(int idComment)
        {
            try
            {
                var result = _forumService.DeleteComment(idComment);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: api/post
        [HttpPost("editComment")]
        public async Task<ActionResult> EditComment(int idComment, String content)
        {
            try
            {
                var result = _forumService.EditComment(idComment, content);
                return Ok(result);
            }
            catch
            {
                return BadRequest();

            }
        }
        [HttpGet("GetAllPostSaved")]
        public async Task<ActionResult> GetAllPostSaved(int userId)
        {
            try
            {
                var data = await _forumService.GetAllPostSaved(userId);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
