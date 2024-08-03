using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Repositories.EventRatingRepository
{
    public class EventRatingRepository : IEventRatingRepository
    {
        private readonly FpttickethubContext _context;

        public EventRatingRepository(FpttickethubContext context)
        {
            _context = context;
        }

        public async Task<object> RatingByRatingId(int ratingId, int userId)
        {
            try
            {
                var rating = await _context.Eventratings
                    .Include(r => r.Event)
                    .Include(r => r.Account)
                    .FirstOrDefaultAsync(r => r.EventRatingId == ratingId);

                if (rating == null)
                {
                    return new
                    {
                        message = "Rating not found",
                        status = 404
                    };
                }

                if (rating.AccountId != userId)
                {
                    return new
                    {
                        message = "Unauthorized access to this rating",
                        status = 403
                    };
                }

                return new
                {
                    message = "Rating retrieved successfully",
                    status = 200,
                    rating = new
                    {
                        rating.EventRatingId,
                        rating.EventId,
                        EventName = rating.Event != null ? rating.Event.EventName : null,
                        rating.AccountId,
                        AccountName = rating.Account?.FullName,
                        rating.Rating,
                        rating.Review,
                        rating.RatingDate,
                        rating.Status
                    }
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = "Failed to retrieve rating",
                    status = 400,
                    error = ex.Message
                };
            }
        }
        public async Task<object> EditEventRating(Eventrating eventRating)
        {
            try
            {
                eventRating.RatingDate = DateTime.UtcNow;
                eventRating.Status = "Active";

                if (eventRating.Rating < 1 || eventRating.Rating > 5)
                {
                    return new
                    {
                        message = "Rating must be between 1 and 5",
                        status = 400
                    };
                }

                var existingRating = await _context.Eventratings
                    .FindAsync(eventRating.EventRatingId);

                if (existingRating == null)
                {
                    _context.Eventratings.Add(eventRating);
                    await _context.SaveChangesAsync();
                    return new
                    {
                        message = "Rating Added",
                        status = 200,
                        eventRating
                    };
                }
                else
                {
                    existingRating.Rating = eventRating.Rating;
                    existingRating.Review = eventRating.Review;
                    existingRating.RatingDate = eventRating.RatingDate;
                    existingRating.Status = eventRating.Status;
                    await _context.SaveChangesAsync();
                    return new
                    {
                        message = "Rating Updated",
                        status = 200,
                        eventRating = existingRating
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    message = "Edit Rating Failed",
                    status = 400,
                    error = ex.Message
                };
            }
        }

        public async Task<object> GetAllRatings()
        {
            try
            {
                var ratings = await _context.Eventratings
                    .Include(r => r.Event)
                    .Include(r => r.Account)
                    .Select(r => new
                    {
                        r.EventRatingId,
                        r.EventId,
                        r.AccountId,
                        AccountName = r.Account.FullName,
                        EventName = r.Event.EventName,
                        r.Rating,
                        r.Review,
                        r.RatingDate,
                        r.Status
                    })
                    .ToListAsync();

                return new
                {
                    message = "Ratings retrieved successfully",
                    status = 200,
                    ratings
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = "Failed to retrieve ratings",
                    status = 400,
                    error = ex.Message
                };
            }
        }

        public async Task<object> UpdateRatingStatus(int ratingId, string status)
        {
            try
            {
                var rating = await _context.Eventratings.FindAsync(ratingId);
                if (rating == null)
                {
                    return new { message = "Rating not found", status = 404 };
                }

                rating.Status = status;
                await _context.SaveChangesAsync();

                return new { message = "Rating status updated successfully", status = 200 };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = "Failed to update rating status",
                    status = 400,
                    error = ex.Message
                };
            }
        }



        public async Task<object> DeleteRating(int ratingId)
        {
            try
            {
                var rating = await _context.Eventratings.FindAsync(ratingId);
                if (rating == null)
                {
                    return new { message = "Rating not found", status = 404 };
                }

                _context.Eventratings.Remove(rating);
                await _context.SaveChangesAsync();

                return new { message = "Rating deleted successfully", status = 200 };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = "Failed to delete rating",
                    status = 400,
                    error = ex.Message
                };
            }
        }
        public async Task<object> GetRateByEventId(int eventId)
        {
            try
            {
                var ratings = await _context.Eventratings
                    .Where(r => r.EventId == eventId)
                    .Include(r => r.Event)
                    .Include(r => r.Account)
                    .Select(r => new
                    {
                        r.EventRatingId,
                        r.EventId,
                        r.AccountId,
                        AccountName = r.Account.FullName,
                        EventName = r.Event.EventName,
                        r.Rating,
                        r.Review,
                        r.RatingDate,
                        r.Status
                    })
                    .ToListAsync();

                return new
                {
                    message = "Ratings retrieved successfully",
                    status = 200,
                    ratings
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = "Failed to retrieve ratings",
                    status = 400,
                    error = ex.Message
                };
            }
        }
        public async Task<object> CheckRatingStatus(int eventRatingId)
        {
            try
            {
                var rating = await _context.Eventratings.FindAsync(eventRatingId);
                if (rating == null)
                {
                    return new
                    {
                        message = "Rating not found",
                        status = 404
                    };
                }

                return new
                {
                    message = "Rating status retrieved successfully",
                    status = 200,
                    ratingStatus = rating.Status
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    message = "Failed to retrieve rating status",
                    status = 400,
                    error = ex.Message
                };
            }
        }
    }
}