using backend.DTOs;
using backend.Services.StatisticService;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        // GET: api/statistics/monthly-revenue
        [HttpGet("monthly-revenue")]
        public async Task<ActionResult<IEnumerable<MonthlyRevenueDTO>>> GetMonthlyRevenue()
        {
            try
            {
                var monthlyRevenue = await _statisticService.GetMonthlyRevenue();
                return Ok(monthlyRevenue);
            }
            catch
            {
                return BadRequest("Unable to fetch monthly revenue.");
            }
        }

        // GET: api/statistics/active-accounts
        [HttpGet("active-accounts")]
        public async Task<ActionResult<MonthlyRegisteredUsersDTO>> GetActiveAccount()
        {
            try
            {
                var activeAccountCount = await _statisticService.GetMonthlyActiveUsers();
                return Ok(activeAccountCount);
            }
            catch
            {
                return BadRequest("Unable to fetch active accounts count.");
            }
        }

        // GET: api/statistics/top-rated-events
        [HttpGet("top-rated-events")]
        public async Task<ActionResult<IEnumerable<TopRatedEventDTO>>> GetTopRatedEvents()
        {
            try
            {
                var topRatedEvents = await _statisticService.GetTopRatedEvents();
                return Ok(topRatedEvents);
            }
            catch
            {
                return BadRequest("Unable to fetch top-rated events.");
            }
        }

        // GET: api/statistics/event-revenue
        [HttpGet("event-revenue")]
        public async Task<ActionResult<IEnumerable<EventRevenueDTO>>> GetEventRevenue()
        {
            try
            {
                var eventRevenues = await _statisticService.GetEventRevenue();
                return Ok(eventRevenues);
            }
            catch
            {
                return BadRequest("Unable to fetch event revenue.");
            }
        }

        // GET: api/statistics/top-participants
        [HttpGet("top-participants")]
        public async Task<ActionResult<IEnumerable<TopParticipantsDTO>>> GetTopParticipants()
        {
            try
            {
                var topParticipants = await _statisticService.GetTopParticipants();
                return Ok(topParticipants);
            }
            catch
            {
                return BadRequest("Unable to fetch top participants.");
            }
        }

        // GET: api/statistics/top-revenue-events
        [HttpGet("top-revenue-events")]
        public async Task<ActionResult<IEnumerable<TopRevenueEventDTO>>> GetTopRevenueEvents()
        {
            try
            {
                var topRevenueEvents = await _statisticService.GetTopRevenueEvents();
                return Ok(topRevenueEvents);
            }
            catch
            {
                return BadRequest("Unable to fetch top revenue events.");
            }
        }

        // GET: api/statistics/top-participants-events
        [HttpGet("top-participants-events")]
        public async Task<ActionResult<IEnumerable<TopParticipantsEventDTO>>> GetTopParticipantsEvents()
        {
            try
            {
                var topParticipantsEvents = await _statisticService.GetTopParticipantsEvents();
                return Ok(topParticipantsEvents);
            }
            catch
            {
                return BadRequest("Unable to fetch top participants events.");
            }
        }

        // GET: api/statistics/report
        [HttpGet("report")]
        public async Task<IActionResult> GenerateEventStatisticsReport()
        {
            try
            {
                var pdfData = await _statisticService.GenerateEventStatisticsReport();
                return File(pdfData, "application/pdf", "EventStatisticsReport.pdf");
            }
            catch
            {
                return BadRequest("Unable to generate statistics report.");
            }
        }
    }
}
