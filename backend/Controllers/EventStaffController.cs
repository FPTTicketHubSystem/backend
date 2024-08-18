using backend.DTOs;
using backend.Models;
using backend.Services.EventService;
using backend.Services.EventStaffService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{

    [Route("api/eventStaff")]
    [ApiController]
    [Authorize(Roles = "Organizer")]
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

        [HttpPost("addStaffByEmail")]
        public async Task<ActionResult> AddStaffByEmail(string email, int eventId)
        {
            try
            {
                var result = _eventStaffService.AddStaffByEmail(email , eventId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("deleteStaff")]
        public async Task<ActionResult> DeleteStaff (int staffId, int eventId)
        {
            try
            {
                var result = _eventStaffService.DeleteStaff(staffId, eventId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        //GET: api/eventStaff
        [HttpGet("getUpcomingEventByOrganizer")]
        public async Task<ActionResult> GetUpcomingEventByOrganizer(int organizerId)
        {
            try
            {
                var data = await _eventStaffService.GetUpcomingEventByOrganizer(organizerId);
                return Ok(data);
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

    }
}
