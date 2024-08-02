using backend.Models;

public interface IEventRatingRepository
{
    Task<object> RatingByRatingId(int ratingId, int userId);
    Task<object> EditEventRating(Eventrating eventRating);
    Task<object> GetAllRatings();
    Task<object> UpdateRatingStatus(int ratingId, string status);
    Task<object> DeleteRating(int ratingId);
    Task<object> GetRateByEventId(int eventId);
    Task<object> CheckRatingStatus(int eventRatingId);
}