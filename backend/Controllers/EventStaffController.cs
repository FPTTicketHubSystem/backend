using backend.DTOs;
using backend.Models;
using backend.Services.EventService;
using backend.Services.EventStaffService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/eventStaff")]
    [ApiController]
    public class EventStaffController : ControllerBase
    {
        private readonly IEventStaffService _eventStaffService;

        public EventStaffController(IEventStaffService eventStaffService)
        {
            _eventStaffService = eventStaffService;
        }

        // POST: api/eventStaff
        [HttpPost("registerStaff")]
        public async Task<ActionResult> RegisterStaff(EventStaff eventStaff)
        {
            try
            {
                var result = _eventStaffService.RegisterStaff(eventStaff);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        //GET: api/eventStaff
        [HttpGet("getStaffByEvent")]
        public async Task<ActionResult> GetStaffByEvent(int eventId)
        {
            try
            {
                var data = await _eventStaffService.GetStaffByEvent(eventId);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

        //POST: api/eventStaff
        [HttpPost("approveStaff")]
        public async Task<ActionResult> ApproveStaff(int accountId, int eventId, string status)
        {
            try
            {
                var result = _eventStaffService.ApproveStaff(accountId, eventId, status);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
