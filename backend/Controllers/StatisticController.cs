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

        //GET: api/statistics/total-revenue
        [HttpGet("total-revenue")]
        public async Task<ActionResult> GetTotalRevenue()
        {
            try
            {
                var totalRevenue = await _statisticService.GetTotalRevenue();
                return Ok(totalRevenue);
            }
            catch
            {
                return BadRequest();
            }
        }

        //GET: api/statistics/total-participants
        [HttpGet("total-participants")]
        public async Task<ActionResult> GetTotalParticipants()
        {
            try
            {
                var totalParticipants = await _statisticService.GetTotalParticipants();
                return Ok(totalParticipants);
            }
            catch
            {
                return BadRequest();
            }
        }

        //GET: api/statistics/top-rated-events
        [HttpGet("top-rated-events")]
        public async Task<ActionResult> GetTopRatedEvents()
        {
            try
            {
                var topRatedEvents = await _statisticService.GetTopRatedEvents();
                return Ok(topRatedEvents);
            }
            catch
            {
                return BadRequest();
            }
        }

        //GET: api/statistics/top-revenue-events
        [HttpGet("top-revenue-events")]
        public async Task<ActionResult> GetTopRevenueEvents()
        {
            try
            {
                var topRevenueEvents = await _statisticService.GetTopRevenueEvents();
                return Ok(topRevenueEvents);
            }
            catch
            {
                return BadRequest();
            }
        }

        //GET: api/statistics/top-participants-events
        [HttpGet("top-participants-events")]
        public async Task<ActionResult> GetTopParticipantsEvents()
        {
            try
            {
                var topParticipantsEvents = await _statisticService.GetTopParticipantsEvents();
                return Ok(topParticipantsEvents);
            }
            catch
            {
                return BadRequest();
            }
        }

        //GET: api/statistics/monthly-revenue
        [HttpGet("monthly-revenue")]
        public async Task<ActionResult> GetMonthlyRevenue()
        {
            try
            {
                var monthlyRevenue = await _statisticService.GetMonthlyRevenue();
                return Ok(monthlyRevenue);
            }
            catch
            {
                return BadRequest();
            }
        }

        //GET: api/statistics/monthly-participants
        [HttpGet("monthly-participants")]
        public async Task<ActionResult> GetMonthlyParticipants()
        {
            try
            {
                var monthlyParticipants = await _statisticService.GetMonthlyParticipants();
                return Ok(monthlyParticipants);
            }
            catch
            {
                return BadRequest();
            }
        }

        /*//GET: api/statistics/yearly-revenue
        [HttpGet("yearly-revenue")]
        public async Task<ActionResult> GetYearlyRevenue()
        {
            try
            {
                var yearlyRevenue = await _statisticService.GetYearlyRevenue();
                return Ok(yearlyRevenue);
            }
            catch
            {
                return BadRequest();
            }
        }

        //GET: api/statistics/yearly-participants
        [HttpGet("yearly-participants")]
        public async Task<ActionResult> GetYearlyParticipants()
        {
            try
            {
                var yearlyParticipants = await _statisticService.GetYearlyParticipants();
                return Ok(yearlyParticipants);
            }
            catch
            {
                return BadRequest();
            }
        }*/

        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportEventStatisticsReport()
        {
            try
            {
                var pdfData = await _statisticService.GenerateEventStatisticsReport();

                return File(pdfData, "application/pdf", "EventStatisticsReport.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while generating the report.");
            }
        }
    }
}
