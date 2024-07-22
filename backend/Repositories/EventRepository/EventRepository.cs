using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Repositories.EventRepository
{
    public class EventRepository : IEventRepository
    {
        private readonly FpttickethubContext _context;

        public EventRepository(FpttickethubContext context)
        {
            _context = context;
        }

        public async Task<object> GetAllEvent()
        {
            var data = _context.Events
                .Include(e => e.Category)
                .Include(e => e.Tickettypes)
                .Include(e => e.Account)
                .OrderByDescending(e => e.StartTime)
                .Select(e =>
                new
                {
                    e.Account.AccountId,
                    e.EventId,
                    e.CategoryId,
                    e.Category.CategoryName,
                    e.Tickettypes,
                    e.Account.FullName,
                    e.Account.Avatar,
                    e.EventName,
                    e.ThemeImage,
                    e.EventDescription,
                    e.Address,
                    e.Location,
                    e.StartTime,
                    e.EndTime,
                    e.Status
                });
            return data;
        }

        public async Task<object> GetAllEventAdmin()
        {
            var data = _context.Events
                .Include(e => e.Category)
                .Include(e => e.Tickettypes)
                .Include(e => e.Account)
                .Where(e => e.Status != "Nháp")
                .OrderByDescending(e => e.StartTime)
                .Select(e =>
                new
                {
                    e.Account.AccountId,
                    e.EventId,
                    e.CategoryId,
                    e.Category.CategoryName,
                    e.Tickettypes,
                    e.Account.FullName,
                    e.Account.Avatar,
                    e.EventName,
                    e.ThemeImage,
                    e.EventDescription,
                    e.Address,
                    e.Location,
                    e.StartTime,
                    e.EndTime,
                    e.Status
                });
            return data;
        }

        public object AddEvent(EventDTO newEventDto)
        {
            try
            {
                var newEvent = new Event
                {
                    AccountId = newEventDto.AccountId,
                    CategoryId = newEventDto.CategoryId,
                    EventName = newEventDto.EventName,
                    ThemeImage = newEventDto.ThemeImage,
                    EventDescription = newEventDto.EventDescription,
                    Address = newEventDto.Address,
                    Location = newEventDto.Location,
                    StartTime = newEventDto.StartTime,
                    EndTime = newEventDto.EndTime,
                    TicketQuantity = newEventDto.TicketQuantity,
                    Status = newEventDto.Status,
                };

                _context.Events.Add(newEvent);
                _context.SaveChanges();

                var eventId = newEvent.EventId;

                var eventImages = newEventDto.EventImages.Select(imageDto => new Eventimage
                {
                    EventId = eventId,
                    ImageUrl = imageDto.ImageUrl,
                    Status = ""
                }).ToList();

                var ticketTypes = newEventDto.TicketTypes.Select(ticketTypeDto => new Tickettype
                {
                    EventId = eventId,
                    TypeName = ticketTypeDto.TypeName,
                    Price = ticketTypeDto.Price,
                    Quantity = ticketTypeDto.Quantity,
                    Status = ""
                }).ToList();

                var discountCodes = newEventDto.DiscountCodes.Select(discountCodeDto => new Discountcode
                {
                    AccountId = newEventDto.AccountId,
                    EventId = eventId,
                    Code = discountCodeDto.Code,
                    DiscountAmount = discountCodeDto.DiscountAmount,
                    Quantity = discountCodeDto.Quantity,
                    Status = ""
                }).ToList();

                _context.Eventimages.AddRange(eventImages);
                _context.Tickettypes.AddRange(ticketTypes);
                _context.Discountcodes.AddRange(discountCodes);

                _context.SaveChanges();
                return new
                {
                    message = "Event Added",
                    status = 200,
                    newEvent
                };
            }
            catch
            {
                return new
                {
                    message = "Add Event Fail",
                    status = 400
                };
            }
        }

        public object EditEvent(int eventId, EventDTO updatedEventDto)
        {
            try
            {
                var existingEvent = _context.Events.FirstOrDefault(e => e.EventId == eventId);

                if (existingEvent == null)
                {
                    return new
                    {
                        message = "Event Not Found",
                        status = 404
                    };
                }

                existingEvent.AccountId = updatedEventDto.AccountId;
                existingEvent.CategoryId = updatedEventDto.CategoryId;
                existingEvent.EventName = updatedEventDto.EventName;
                existingEvent.ThemeImage = updatedEventDto.ThemeImage;
                existingEvent.EventDescription = updatedEventDto.EventDescription;
                existingEvent.Address = updatedEventDto.Address;
                existingEvent.Location = updatedEventDto.Location;
                existingEvent.StartTime = updatedEventDto.StartTime;
                existingEvent.EndTime = updatedEventDto.EndTime;
                existingEvent.TicketQuantity = updatedEventDto.TicketQuantity;
                existingEvent.Status = updatedEventDto.Status;

                var existingEventImages = _context.Eventimages.Where(ei => ei.EventId == eventId).ToList();
                _context.Eventimages.RemoveRange(existingEventImages);

                var updatedEventImages = updatedEventDto.EventImages.Select(imageDto => new Eventimage
                {
                    EventId = eventId,
                    ImageUrl = imageDto.ImageUrl,
                    Status = ""
                }).ToList();
                _context.Eventimages.AddRange(updatedEventImages);

                var existingTicketTypes = _context.Tickettypes.Where(tt => tt.EventId == eventId).ToList();
                _context.Tickettypes.RemoveRange(existingTicketTypes);

                var updatedTicketTypes = updatedEventDto.TicketTypes.Select(ticketTypeDto => new Tickettype
                {
                    EventId = eventId,
                    TypeName = ticketTypeDto.TypeName,
                    Price = ticketTypeDto.Price,
                    Quantity = ticketTypeDto.Quantity,
                    Status = ""
                }).ToList();
                _context.Tickettypes.AddRange(updatedTicketTypes);

                var existingDiscountCodes = _context.Discountcodes.Where(dc => dc.EventId == eventId).ToList();
                _context.Discountcodes.RemoveRange(existingDiscountCodes);

                var updatedDiscountCodes = updatedEventDto.DiscountCodes.Select(discountCodeDto => new Discountcode
                {
                    AccountId = updatedEventDto.AccountId,
                    EventId = eventId,
                    Code = discountCodeDto.Code,
                    DiscountAmount = discountCodeDto.DiscountAmount,
                    Quantity = discountCodeDto.Quantity,
                    Status = ""
                }).ToList();
                _context.Discountcodes.AddRange(updatedDiscountCodes);

                _context.SaveChanges();

                return new
                {
                    message = "Event Updated",
                    status = 200,
                    existingEvent
                };
            }
            catch
            {
                return new
                {
                    message = "Edit Event Fail",
                    status = 400
                };
            }
        }

        public object GetEventById (int eventId)
        {
            var data = _context.Events
                .Include(e => e.Eventratings)
                .Include(e=> e.Tickettypes)
                .Include(e=> e.Discountcodes)
                .FirstOrDefault(e => e.EventId == eventId);
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public object GetEventByCategory (int categoryId)
        {
            var data = _context.Events
                .Include(e => e.Tickettypes)
                .Include(e => e.Category)
                .Where(e => e.CategoryId == categoryId && e.Status == "Đã duyệt")
                .OrderByDescending(e => e.StartTime)
                .ToList();
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public async Task<object> GetUpcomingEvent()
        {
            var data = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Tickettypes)
                .Include(e => e.Account)
                .Where(e => e.Status == "Đã duyệt" && e.StartTime > DateTime.Now)
                .OrderByDescending(e => e.StartTime)
                .Take(5)
                .Select(e =>
                new
                {
                    e.Account.AccountId,
                    e.EventId,
                    e.CategoryId,
                    e.Category.CategoryName,
                    e.Tickettypes,
                    e.Account.FullName,
                    e.Account.Avatar,
                    e.EventName,
                    e.ThemeImage,
                    e.EventDescription,
                    e.Address,
                    e.Location,
                    e.StartTime,
                    e.EndTime,
                    e.Status
                })
                .ToListAsync();

            return data;
        }

        public async Task<object> ChangeEventStatus(int eventId, string status)
        {
            try
            {
                var existingEvent = await _context.Events
                    .Include(e => e.Account)
                    .FirstOrDefaultAsync(e => e.EventId == eventId);
                if (existingEvent == null)
                {
                    return new
                    {
                        message = "NotFound",
                        status = 400
                    };
                }
                existingEvent.Status = status;
                if (status == "Đã duyệt")
                {
                    existingEvent.Account.RoleId = 3;
                }
                await _context.SaveChangesAsync();
                return new
                {
                    message = "Status changed",
                    status = 200,
                    existingEvent
                };
            }
            catch
            {
                return new
                {
                    message = "Fail to change status",
                    status = 400
                };
            }
        }

    }
}
