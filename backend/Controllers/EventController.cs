using System.Collections.Generic;
using System.Threading.Tasks;
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

    }
}
