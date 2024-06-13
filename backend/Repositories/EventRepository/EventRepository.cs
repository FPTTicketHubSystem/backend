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
                    Status = "",
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

    }
}
