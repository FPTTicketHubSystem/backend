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

        [HttpGet("getAllNewsAdmin")]
        public async Task<ActionResult> GetAllNewsAdmin([FromQuery] string status = "")
        {
            try
            {
                var data = await _newService.GetAllNewsAdmin(status);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

        /*[HttpGet("getAllNews")]
        public async Task<ActionResult> GetAllNews()
        {
            try
            {
                var result = _newService.GetAllNews();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }*/

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

        [HttpGet("getNewsById/{newsId}")]
        public async Task<ActionResult> GetNewsById(int newsId)
        {
            try
            {
                var news = await _newService.GetNewsById(newsId);
                if (news == null)
                {
                    return NotFound();
                }
                return Ok(news);
            }
            catch
            {
                return BadRequest();
            }
        }

        /*// GET: api/news 
        [HttpGet("getNewsById")]
        public async Task<ActionResult> GetNewsById(int newsId)
        {
            try
            {
                var data = _newService.GetNewsById(newsId);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }*/

        /*        //GET: api/news
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
                }*/
        [HttpPost("changeStatusNews")]
        public async Task<ActionResult> ChangeStatusNews(int newsId, string status)
        {
            try
            {
                var result = _newService.ChangeStatusNews(newsId, status);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("displayNewDetail")]
        public async Task<ActionResult> DisplayNewDetail(int newsId)
        {
            try
            {
                var result = _newService.GetNewDetail(newsId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("getNewsInPage")]
        public async Task<ActionResult> GetNewsInPage(int page = 1, int pageSize = 3)
        {
            try
            {
                var result = _newService.GetNewsByPage(page, pageSize);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("getNewsForEdit")]
        public async Task<ActionResult> GetNewsForEdit(int newsId)
        {
            try
            {
                var data = _newService.GetNewsForEdit(newsId);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }

        [HttpPut("editNews")]
        public async Task<ActionResult> EditNews(NewsDTO updateNews)
        {
            try
            {
                var result = _newService.EditNew(updateNews);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}

