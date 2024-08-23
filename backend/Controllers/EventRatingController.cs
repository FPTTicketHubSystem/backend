using backend.Models;
using backend.Repositories.EventRatingRepository;
using backend.Services.EventRatingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

public class EventRatingController : ControllerBase
{
    private readonly IEventRatingService _eventRatingService;

    public EventRatingController(IEventRatingService eventRatingService)
    {
        _eventRatingService = eventRatingService;
    }

    [Authorize(Roles = "User")]
    [HttpGet("ratingByRatingId")]
    public async Task<ActionResult> RatingByRatingId(int ratingId, int userId)
    {
        try
        {
            var data = await _eventRatingService.RatingByRatingId(ratingId, userId);
            if (data == null)
            {
                return NotFound(new { message = "Rating not found or does not belong to the user" });
            }
            return Ok(data);
        }
        catch
        {
            return BadRequest();
        }
    }

    [Authorize(Roles = "User")]
    [HttpPost("editEventRating")]
    public async Task<ActionResult> EditEventRating([FromBody] Eventrating eventRating)
    {
        try
        {
            var result = await _eventRatingService.EditEventRating(eventRating);
            return Ok(result);
        }
        catch
        {
            return BadRequest(new { message = "Failed to edit event rating." });
        }
    }

    // Thêm các endpoint mới cho Admin
    [Authorize(Roles = "Admin")]
    [HttpGet("getall")]
    public async Task<ActionResult> GetAllRatings()
    {
        try
        {
            var result = await _eventRatingService.GetAllRatings();
            return Ok(result);
        }
        catch
        {
            return BadRequest();
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("updateRatingStatus")]
    public async Task<ActionResult> UpdateRatingStatus(int ratingId, string status)
    {
        try
        {
            var result = await _eventRatingService.UpdateRatingStatus(ratingId, status);
            return Ok(result);
        }
        catch
        {
            return BadRequest(new { message = "Failed to update rating status." });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("deleteRating")]
    public async Task<ActionResult> DeleteRating(int ratingId)
    {
        try
        {
            var result = await _eventRatingService.DeleteRating(ratingId);
            return Ok(result);
        }
        catch
        {
            return BadRequest(new { message = "Failed to delete rating." });
        }
    }

    [Authorize(Roles = "Admin,Organizer")]
    [HttpGet("getRateByEventId")]
    public async Task<IActionResult> GetRateByEventId(int eventId)
    {
        var result = await _eventRatingService.GetRateByEventId(eventId);
        return Ok(result);
    }

    [HttpGet("check-status/{eventRatingId}")]
    public async Task<IActionResult> CheckRatingStatus(int eventRatingId)
    {
        var result = await _eventRatingService.CheckRatingStatus(eventRatingId);
        var statusCode = (int)result.GetType().GetProperty("status").GetValue(result, null);

        if (statusCode == 200)
        {
            return Ok(result);
        }
        else if (statusCode == 404)
        {
            return NotFound(result);
        }
        else
        {
            return BadRequest(result);
        }
    }
}