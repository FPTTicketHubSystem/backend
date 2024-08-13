using backend.Services.NewsService;
using backend.Services.TicketService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/ticket")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        [HttpGet("getTicketByAccount")]
        public async Task<ActionResult> GetTicketByAccount(int accountId)
        {
            try
            {
                var data = await _ticketService.GetTicketByAccount(accountId);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("getTicketById")]
        public async Task<ActionResult> GetTicketById(int ticketId, int userId)
        {
            try
            {
                var data = _ticketService.GetTicketById(ticketId, userId);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }

    }
}
