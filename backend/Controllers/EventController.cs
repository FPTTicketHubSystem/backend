using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Models;
using backend.Services.EventService;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        //GET: api/event
        [HttpGet("getAllEvent")]
        public async Task<ActionResult> GetAllEvent()
        {
            try
            {
                var data = await _eventService.GetAllEvent();
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }


        // POST: api/event
        [HttpPost("addEvent")]
        public async Task<ActionResult> AddEvent(EventDTO newEvent)
        {
            try
            {
                var result = _eventService.AddEvent(newEvent);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: api/event
        [HttpPost("editEvent")]
        public async Task<ActionResult> EditEvent(int eventId, EventDTO updatedEventDto)
        {
            try
            {
                var result = _eventService.EditEvent(eventId, updatedEventDto);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: api/event 
        [HttpGet("getEventById")]
        public async Task<ActionResult> GetEventById(int eventId)
        {
            try
            {
                var data = _eventService.GetEventById(eventId);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }

        // GET: api/event
        [HttpGet("getEventByCategory")]
        public async Task<ActionResult> GetEventByCategory(int categoryId)
        {
            try
            {
                var data = _eventService.GetEventByCategory(categoryId);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }

        //GET: api/event
        [HttpGet("getUpcomingEvent")]
        public async Task<ActionResult> GetUpcomingEvent()
        {
            try
            {
                var data = await _eventService.GetUpcomingEvent();
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
