using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using backend.Services.EventService;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Organizer")]
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

        //GET: api/event
        [Authorize(Roles = "Admin")]
        [HttpGet("getAllEventAdmin")]
        public async Task<ActionResult> GetAllEventAdmin()
        {
            try
            {
                var data = await _eventService.GetAllEventAdmin();
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }


        // POST: api/event
        [Authorize(Roles = "Organizer,User")]
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
        [Authorize(Roles = "Organizer,User")]
        [HttpPost("editEvent")]
        public async Task<ActionResult> EditEvent(EventDTO updatedEventDto)
        {
            try
            {
                var result = _eventService.EditEvent(updatedEventDto);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: api/event
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        [Authorize(Roles = "Admin")]
        [HttpPost("changeEventStatus")]
        public async Task<ActionResult> ChangeEventStatus(int eventId, string status)
        {
            try
            {
                var data = await _eventService.ChangeEventStatus(eventId, status);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

        //GET: api/event
        [Authorize(Roles = "Organizer,User")]
        [HttpGet("getEventByAccount")]
        public async Task<ActionResult> GetEventByAccount(int accountId)
        {
            try
            {
                var data = await _eventService.GetEventByAccount(accountId);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: api/event 
        [Authorize(Roles = "Organizer,User")]
        [HttpGet("getEventForEdit")]
        public async Task<ActionResult> GetEventForEdit(int eventId)
        {
            try
            {
                var data = _eventService.GetEventForEdit(eventId);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }

        //update for organizer manage
        [Authorize(Roles = "Organizer,User")]
        [HttpGet("getTicketTypeByEvent")]
        public async Task<ActionResult> GetTicketTypeByEvent(int eventId)
        {
            try
            {
                var data = await _eventService.GetTicketTypeByEvent(eventId);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }

        [Authorize(Roles = "Organizer,User")]
        [HttpPut("updateTicketQuantity")]
        public async Task<ActionResult> UpdateTicketQuantity(int ticketTypeId, int addQuantity, int addOrRemove)
        {
            try
            {
                var data = _eventService.UpdateTicketQuantity(ticketTypeId, addQuantity, addOrRemove);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }

        [Authorize(Roles = "Organizer,User")]
        [HttpGet("getDiscountCodeByEvent")]
        public async Task<ActionResult> GetDiscountCodeByEvent(int eventId)
        {
            try
            {
                var data = _eventService.GetDiscountCodeByEvent(eventId);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }
        // POST: api/event
        [Authorize(Roles = "Organizer,User")]
        [HttpPost("addDiscountCode")]
        public async Task<ActionResult> AddDiscountCode(DiscountCodeDTO discountcode)
        {
            try
            {
                var result = _eventService.AddDiscountCode(discountcode);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Organizer,User")]
        [HttpPut("updateDiscountQuantity")]
        public async Task<ActionResult> UpdateDiscountQuantity(int discountId, int addQuantity)
        {
            try
            {
                var data = _eventService.UpdateDiscountQuantity(discountId, addQuantity);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }

        [Authorize(Roles = "Organizer")]
        [HttpGet("getEventStatistics")]
        public async Task<ActionResult> GetEventStatistics(int eventId)
        {
            try
            {
                var numOfTicketSold = _eventService.GetNumberOfTicketSold(eventId);
                var totalRevenue = _eventService.GetTotalRevenue(eventId);
                var actualParticipants = _eventService.GetActualParticipants(eventId);
                var ticketSalesPerTicketType = await _eventService.GetTicketSalesPerTicketType(eventId);
                var eventStatus = await _eventService.GetEventStatus(eventId);
                var eventRating = await _eventService.GetAverageRating(eventId);
                return Ok(new
                {
                    status = 200,
                    numOfTicketSold,
                    totalRevenue,
                    actualParticipants,
                    ticketSalesPerTicketType,
                    eventStatus,
                    eventRating
                });
            }
            catch { return BadRequest(); }
        }

        [AllowAnonymous]
        [HttpGet("searchEventByContainTiTile")]
        public async Task<ActionResult> SearchEventByContainTiTile(string searchString)
        {
            try
            {
                var data = _eventService.searchEventByContainTiTile(searchString);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }

        [AllowAnonymous]
        [HttpGet("searchEventByFilter")]
        public async Task<ActionResult> SearchEventByFilter(string filter)
        {
            try
            {
                var data = _eventService.searchEventByFilter(filter);
                return Ok(data);
            }
            catch { return BadRequest(); }
        }


        //GET: api/event
        [AllowAnonymous]
        [HttpGet("getAllEventUser")]
        public async Task<ActionResult> GetAllEventUser()
        {
            try
            {
                var data = await _eventService.GetAllEventUser();
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

        //update for organizer
        [Authorize(Roles = "Organizer,User")]
        [HttpPost("addTicketType")]
        public async Task<ActionResult> AddTicketType(TicketTypeDTO ticketType)
        {
            try
            {
                var result = _eventService.AddTicketType(ticketType);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}