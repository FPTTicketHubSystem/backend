using backend.Services.StaffService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/staff")]
    [ApiController]
    [Authorize(Roles = "OrganizerStaff")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet("checkInTicket")]
        public async Task<ActionResult> CheckInTicket (int ticketId, int staffId)
        {
            try
            {
                var data = _staffService.CheckInTicket(ticketId, staffId);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("checkinHistory")]
        public async Task<ActionResult> GetCheckinHistoryByEvent(int staffId)
        {
            try
            {
                var data = await _staffService.GetCheckinHistoryByEvent(staffId);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("getEventByStaff")]
        public async Task<ActionResult> GetEventByStaff(int staffId)
        {
            try
            {
                var data = _staffService.GetEventByStaff(staffId);
                return Ok(data);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
