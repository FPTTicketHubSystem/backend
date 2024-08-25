using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Helper;

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
                        rating.Event.EventName,
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
                var existingRating = await _context.Eventratings.FindAsync(eventRating.EventRatingId);

                if (existingRating == null)
                {
                    return new
                    {
                        message = "NotFound",
                        status = 404
                    };
                }
                else
                {
                    if (DateTime.UtcNow - existingRating.RatingDate > TimeSpan.FromHours(24))
                    {
                        return new
                        {
                            message = "Không thể chỉnh sửa đánh giá sau 24 giờ",
                            status = 400
                        };
                    }

                    existingRating.Rating = eventRating.Rating;
                    existingRating.Review = StringHelpers.NormalizeSpaces(eventRating.Review);
                    existingRating.RatingDate = DateTime.UtcNow;
                    existingRating.Status = "Active";
                }

                if (eventRating.Rating < 1 || eventRating.Rating > 5)
                {
                    return new
                    {
                        message = "Rating must be between 1 and 5",
                        status = 400
                    };
                }

                await _context.SaveChangesAsync();
                return new
                {
                    message = existingRating == null ? "Rating Added" : "Rating Updated",
                    status = 200,
                    eventRating = existingRating ?? eventRating
                };
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