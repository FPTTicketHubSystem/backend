using backend.DTOs;
using backend.Models;
using backend.Services.PostCommentService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/comment")]
    [ApiController]
    //user
    public class PostCommentController : ControllerBase
    {
        private readonly IPostCommentService _postcommentService;
        private readonly IConfiguration _configuration;
        public PostCommentController(IConfiguration configuration, IPostCommentService postcommentService)
        {
            _postcommentService = postcommentService;
            _configuration = configuration;
        }
        [HttpPost("AddComment")]
        public async Task<ActionResult> AddPostcomment([FromBody] PostCommentDTO addPostcomment)
        {
            try
            {
                var postcomment = new Postcomment();
                postcomment.PostId = addPostcomment.PostId;
                postcomment.AccountId = addPostcomment.AccountId;
                postcomment.Content = addPostcomment.Content;
                postcomment.FileComment = addPostcomment.FileComment;
                postcomment.Status = "Đã bình luận";
                postcomment.CommentDate = DateTime.Now;
                var result = _postcommentService.AddPostcomment(postcomment);

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("getCommentByPost")]
        public ActionResult GetCommentbyPost(int postId)
        {
            try
            {
                var data = _postcommentService.GetCommentByPost(postId);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("ChangeStatusPostcomment")]
        public async Task<ActionResult> ChangeStatusPostcomment(int postcommentId, string status)
        {
            try
            {
                var result = _postcommentService.ChangeStatusPostcomment(postcommentId, status);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("EditComment")]
        public async Task<ActionResult> EditComment(EditCommentDTO postcomment)
        {
            try
            {
                var result = _postcommentService.EditComment(postcomment);
                return Ok(result);
            }
            catch { return BadRequest(); }
        }

        [HttpPost("DeleteComment")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                var result = _postcommentService.DeleteComment(commentId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
