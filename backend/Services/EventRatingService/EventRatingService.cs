using backend.Models;
using backend.Repositories.EventRatingRepository;

namespace backend.Services.EventRatingService
{
    public class EventRatingService : IEventRatingService
    {
        private readonly IEventRatingRepository _eventRatingRepository;

        public EventRatingService(IEventRatingRepository eventRatingRepository)
        {
            _eventRatingRepository = eventRatingRepository;
        }

        public async Task<object> RatingByRatingId(int ratingId, int userId)
        {
            return await _eventRatingRepository.RatingByRatingId(ratingId, userId);
        }

        public async Task<object> EditEventRating(Eventrating eventRating)
        {
            return await _eventRatingRepository.EditEventRating(eventRating);
        }

        public async Task<object> GetAllRatings()
        {
            return await _eventRatingRepository.GetAllRatings();
        }

        public async Task<object> UpdateRatingStatus(int ratingId, string status)
        {
            return await _eventRatingRepository.UpdateRatingStatus(ratingId, status);
        }

        public async Task<object> DeleteRating(int ratingId)
        {
            return await _eventRatingRepository.DeleteRating(ratingId);
        }
        public async Task<object> GetRateByEventId(int eventId)
        {
            return await _eventRatingRepository.GetRateByEventId(eventId);
        }
        public async Task<object> CheckRatingStatus(int eventRatingId)
        {
            return await _eventRatingRepository.CheckRatingStatus(eventRatingId);
        }
    }
}