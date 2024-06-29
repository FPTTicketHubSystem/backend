using backend.DTOs;
using backend.Models;
using backend.Services.NewsService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newService;
        public NewsController(INewsService newService)
        {
            _newService = newService;
        }

        //GET: api/news
        [HttpGet("getAllNews")]
        public async Task<ActionResult> GetAllNews()
        {
            try
            {
                var data = await _newService.GetAllNews();
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

        //GET: api/news
        [HttpGet("getNewsByAccount")]
        public async Task<ActionResult> GetNewsByAccount(int accountId)
        {
            try
            {
                var data = await _newService.GetNewsByAccount(accountId);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: api/news
        [HttpPost("addNews")]
        public async Task<ActionResult> AddNews(NewsDTO newsDTO)
        {
            try
            {
                var result = _newService.AddNews(newsDTO);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: api/news 
        [HttpGet("getNewsById")]
        public async Task<ActionResult> GetNewsById(int newsId)
        {
            try
            {
                var data = _newService.GetNewsById(newsId);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }

        //GET: api/news
        [HttpGet("getLastestNews")]
        public async Task<ActionResult> GetLastestNews()
        {
            try
            {
                var data = await _newService.GetLastestNews();
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
