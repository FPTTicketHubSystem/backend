using backend.Services.StatisticService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }
        [HttpGet("total-participants/{eventId}")]
        public IActionResult GetTotalParticipants(int eventId)
        {
            try
            {
                var totalParticipants = _statisticService.GetTotalParticipants(eventId);
                return Ok(new { message = "Successfully", status = 200, totalParticipants });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Failed", status = 400 });
            }
        }
        [HttpGet("user-event-count/{userId}")]
        public IActionResult GetUserEventCount(int userId)
        {
            try
            {
                var userEventCount = _statisticService.GetUserEventCount(userId);
                return Ok(new { message = "Successfully", status = 200, userEventCount });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Failed", status = 400 });
            }
        }

        [HttpGet("total-income/{eventId}")]
        public IActionResult GetTotalIncome(int eventId)
        {
            try
            {
                var totalIncome = _statisticService.GetTotalIncome(eventId);
                return Ok(new { message = "Successfully", status = 200, totalIncome });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Failed", status = 400 });
            }
        }

        [HttpGet("total-tickets-sold/{eventId}")]
        public IActionResult GetTotalTicketsSold(int eventId)
        {
            try
            {
                var totalTicketsSold = _statisticService.GetTotalTicketsSold(eventId);
                return Ok(new { message = "Successfully", status = 200, totalTicketsSold });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Failed", status = 400 });
            }
        }
    }
}
