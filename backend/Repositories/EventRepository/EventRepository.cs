using backend.Models;
using backend.Services.OtherService;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
                .OrderBy(e => e.StartTime)
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
                .OrderBy(e => e.StartTime)
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
                if (!newEventDto.TicketTypes.Any())
                {
                    return new
                    {
                        message = "Tickettype is required",
                        status = 400
                    };
                }

                if (string.IsNullOrEmpty(newEventDto.EventDescription))
                {
                    return new
                    {
                        message = "Description is required",
                        status = 400
                    };
                }

                //if (!newEventDto.EventImages.Any())
                //{
                //    return new
                //    {
                //        status = 400,
                //        message = "Doc is required"
                //    };
                //}
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
                    Status = newEventDto.Status,
                };

                _context.Events.Add(newEvent);
                _context.SaveChanges();

                var eventId = newEvent.EventId;

                //var eventImages = newEventDto.EventImages.Select(imageDto => new Eventimage
                //{
                //    EventId = eventId,
                //    ImageUrl = imageDto.ImageUrl,
                //    Status = ""
                //}).ToList();

                var ticketTypes = newEventDto.TicketTypes.Select(ticketTypeDto => new Tickettype
                {
                    EventId = eventId,
                    TypeName = ticketTypeDto.TypeName,
                    Price = ticketTypeDto.Price,
                    Quantity = ticketTypeDto.Quantity,
                    Status = ""
                }).ToList();

                //var discountCodes = newEventDto.DiscountCodes.Select(discountCodeDto => new Discountcode
                //{
                //    AccountId = newEventDto.AccountId,
                //    EventId = eventId,
                //    Code = discountCodeDto.Code,
                //    DiscountAmount = discountCodeDto.DiscountAmount,
                //    Quantity = discountCodeDto.Quantity,
                //    Status = ""
                //}).ToList();

                //_context.Eventimages.AddRange(eventImages);
                _context.Tickettypes.AddRange(ticketTypes);
                //_context.Discountcodes.AddRange(discountCodes);

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

        public object GetEventForEdit(int eventId)
        {
            var data = _context.Events
                .Include(e => e.Eventratings)
                .Include(e => e.Tickettypes)
                .Include(e => e.Discountcodes)
                .Where(e => e.EventId == eventId)
                .Select(e =>
                new
                {
                    e.EventId,
                    e.AccountId,
                    e.CategoryId,
                    e.EventName,
                    e.ThemeImage,
                    e.EventDescription,
                    e.Address,
                    e.Location,
                    e.StartTime,
                    e.EndTime,
                    e.Status,
                    //e.Eventimages,
                    e.Tickettypes,
                    e.Discountcodes,
                }).SingleOrDefault();
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public object EditEvent(EventDTO updatedEventDto)
        {
            try
            {
                var existingEvent = _context.Events.FirstOrDefault(e => e.EventId == updatedEventDto.EventId);

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
                existingEvent.Status = updatedEventDto.Status;

                //var existingEventImages = _context.Eventimages.Where(ei => ei.EventId == updatedEventDto.EventId).ToList();
                //_context.Eventimages.RemoveRange(existingEventImages);

                //var updatedEventImages = updatedEventDto.EventImages.Select(imageDto => new Eventimage
                //{
                //    EventId = updatedEventDto.EventId,
                //    ImageUrl = imageDto.ImageUrl,
                //    Status = ""
                //}).ToList();
                //_context.Eventimages.AddRange(updatedEventImages);

                //var existingTicketTypes = _context.Tickettypes.Where(tt => tt.EventId == updatedEventDto.EventId).ToList();
                //_context.Tickettypes.RemoveRange(existingTicketTypes);

                //var updatedTicketTypes = updatedEventDto.TicketTypes.Select(ticketTypeDto => new Tickettype
                //{
                //    EventId = updatedEventDto.EventId,
                //    TypeName = ticketTypeDto.TypeName,
                //    Price = ticketTypeDto.Price,
                //    Quantity = ticketTypeDto.Quantity,
                //    Status = ""
                //}).ToList();
                //_context.Tickettypes.AddRange(updatedTicketTypes);

                //var existingDiscountCodes = _context.Discountcodes.Where(dc => dc.EventId == updatedEventDto.EventId).ToList();
                //_context.Discountcodes.RemoveRange(existingDiscountCodes);

                //var updatedDiscountCodes = updatedEventDto.DiscountCodes.Select(discountCodeDto => new Discountcode
                //{
                //    AccountId = updatedEventDto.AccountId,
                //    EventId = updatedEventDto.EventId,
                //    Code = discountCodeDto.Code,
                //    DiscountAmount = discountCodeDto.DiscountAmount,
                //    Quantity = discountCodeDto.Quantity,
                //    Status = ""
                //}).ToList();
                //_context.Discountcodes.AddRange(updatedDiscountCodes);

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

        public object GetEventById(int eventId)
        {
            var data = _context.Events
                .Include(e => e.Eventratings)
                .Include(e => e.Tickettypes)
                .Include(e => e.Discountcodes)
                .Where(e => e.EventId == eventId && e.Status == "Đã duyệt")
                .Select(e =>
                new
                {
                    e.EventId,
                    e.CategoryId,
                    e.Category.CategoryName,
                    e.Tickettypes,
                    e.Account.FullName,
                    e.Account.Avatar,
                    e.Account.BirthDay,
                    e.EventName,
                    e.ThemeImage,
                    e.EventDescription,
                    e.Address,
                    e.Location,
                    e.StartTime,
                    e.EndTime,
                }).SingleOrDefault();
            if (data == null)
            {
                return new
                {
                    status = 404,
                    message = "NotFound"
                };
            }
            return data;
        }

        public object GetEventByCategory(int categoryId)
        {
            var data = _context.Events
                .Include(e => e.Tickettypes)
                .Include(e => e.Category)
                .Where(e => e.CategoryId == categoryId && e.Status == "Đã duyệt")
                .OrderBy(e => e.StartTime)
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
                .Where(e => e.Status == "Đã duyệt" && e.StartTime > DateTime.UtcNow)
                .OrderBy(e => e.StartTime)
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
            if (!data.Any() && data.Count < 3)
            {
                data = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Tickettypes)
                .Include(e => e.Account)
                .Where(e => e.Status == "Đã duyệt")
                .OrderBy(e => e.StartTime)
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
            }

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
                    var roleId = existingEvent.Account.RoleId;
                    if (roleId == 3)
                    {
                        EmailService.Instance.SendEventApproveEmail(existingEvent.Account.Email, 2, existingEvent.Account.FullName, existingEvent.EventName);
                    }
                    else
                    {
                        existingEvent.Account.RoleId = 3;
                        EmailService.Instance.SendEventApproveEmail(existingEvent.Account.Email, 1, existingEvent.Account.FullName, existingEvent.EventName);
                    }
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

        public async Task<object> GetEventByAccount(int accountId)
        {
            var data = _context.Events
                .Include(e => e.Category)
                .Include(e => e.Tickettypes)
                .Include(e => e.Account)
                .Where(e => e.AccountId == accountId)
                .OrderBy(e => e.StartTime)
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
            if (data == null)
            {
                return null;
            }
            return data;
        }

        //update for event organizer manage
        public async Task<object> GetTicketTypeByEvent(int eventId)
        {
            var data = _context.Tickettypes
                .Where(tt => tt.EventId == eventId)
                .Select(tt =>
                new
                {
                    tt.TicketTypeId,
                    tt.EventId,
                    tt.TypeName,
                    tt.Price,
                    tt.Quantity
                });
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public async Task<object> UpdateTicketQuantity(int ticketTypeId, int addQuantity)
        {
            var data = _context.Tickettypes.Find(ticketTypeId);
            if (data == null)
            {
                return new
                {
                    message = "NotFound",
                    status = 400
                };
            }
            if ((data.Quantity + addQuantity) < 0)
            {
                return new
                {
                    message = "InvalidQuantity",
                    status = 400
                };
            }
            data.Quantity += addQuantity;
            _context.Tickettypes.Update(data);
            _context.SaveChanges();
            return new
            {
                message = "QuantityUpdated",
                status = 200,
                data
            };
        }



        public async Task<object> GetDiscountCodeByEvent(int eventId)
        {
            var data = _context.Discountcodes
                .Include(d => d.Event)
                .Where(d => d.EventId == eventId)
                .Select(d =>
                new
                {
                    d.DiscountCodeId,
                    d.EventId,
                    d.Code,
                    d.DiscountAmount,
                    d.Quantity
                });
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public object AddDiscountCode(DiscountCodeDTO discountcode)
        {
            try
            {
                var existingDiscountCode = _context.Discountcodes.FirstOrDefault(dc => dc.EventId == discountcode.EventId && dc.Code == discountcode.Code);
                if (existingDiscountCode != null)
                {
                    return new
                    {
                        message = "DiscountCodeIsExist",
                        status = 400
                    };
                }

                var newDiscountCode = new Discountcode
                {
                    AccountId = discountcode.AccountId,
                    EventId = discountcode.EventId,
                    Code = discountcode.Code,
                    DiscountAmount = discountcode.DiscountAmount,
                    Quantity = discountcode.Quantity

                };

                _context.Discountcodes.Add(newDiscountCode);
                _context.SaveChanges();

                return new
                {
                    message = "Add Discount Success",
                    status = 200,
                    newDiscountCode
                };
            }
            catch
            {
                return new
                {
                    message = "Add Discount Fail",
                    status = 400
                };
            }
        }

        public async Task<object> UpdateDiscountQuantity(int discountId, int addQuantity)
        {
            var data = _context.Discountcodes.Find(discountId);
            if (data == null)
            {
                return new
                {
                    message = "NotFound",
                    status = 400
                };
            }
            if ((data.Quantity + addQuantity) < 0)
            {
                return new
                {
                    message = "InvalidQuantity",
                    status = 400
                };
            }
            data.Quantity += addQuantity;
            _context.Discountcodes.Update(data);
            _context.SaveChanges();
            return new
            {
                message = "QuantityUpdated",
                status = 200,
                data
            };
        }

        public int GetNumberOfTicketSold(int eventId)
        {
            var ticketsSold = _context.Tickets
                    .Where(t => t.OrderDetail.Order.Orderdetails.Any(od => od.TicketType.EventId == eventId) && t.OrderDetail.Order.Status == "Đã thanh toán")
                    .Distinct()
                    .Count();
            return ticketsSold;
        }

        public decimal GetTotalRevenue(int eventId)
        {
            var totalRevenue = _context.Payments
                .Where(p => p.Order.Orderdetails.Any(od => od.TicketType.EventId == eventId))
                .Sum(p => p.PaymentAmount);
            return totalRevenue.Value;
        }

        public int GetActualParticipants(int eventId)
        {
            var actualParticipants = _context.Tickets.Include(t => t.OrderDetail)
                                                     .Where(t => t.OrderDetail.TicketType.Event.EventId == eventId && t.IsCheckedIn == true)
                                                     .Sum(t => t.OrderDetail.Quantity);

            return (int)actualParticipants;
        }

        public class TicketSalesPerTicketType
        {
            public string TicketType { get; set; }
            public int NumberOfTicketsSold { get; set; }
            public int RemainingTickets { get; set; }

        }

        public async Task<List<TicketSalesPerTicketType>> GetTicketSalesPerTicketType(int eventId)
        {
            var ticketSalesPerTicketType = await (
                from orderDetail in _context.Orderdetails
                join ticketType in _context.Tickettypes on orderDetail.TicketTypeId equals ticketType.TicketTypeId
                where ticketType.EventId == eventId
                group orderDetail by ticketType into g
                select new TicketSalesPerTicketType
                {
                    TicketType = g.Key.TypeName,
                    NumberOfTicketsSold = (int)g.Sum(od => od.Quantity),
                    RemainingTickets = (int)g.Key.Quantity
                }
            ).ToListAsync();

            return ticketSalesPerTicketType;
        }

        public async Task<object> GetEventStatus(int eventId)
        {
            var eventInfo = await _context.Events
            .Where(e => e.EventId == eventId)
            .FirstOrDefaultAsync();

            if (eventInfo == null)
            {
                return new { status = 404, message = "NotFound" };
            }
            var now = DateTime.UtcNow;
            string eventStatus;

            if (now < eventInfo.StartTime)
            {
                eventStatus = "Chưa diễn ra";
            }
            else if (now >= eventInfo.StartTime && now <= eventInfo.EndTime)
            {
                eventStatus = "Đang diễn ra";
            }
            else
            {
                eventStatus = "Đã kết thúc";
            }
            return new
            {
                eventName = eventInfo.EventName,
                eventStatus = eventStatus,
                organizerId = eventInfo.AccountId
            };

        }

        public async Task<object> GetAverageRating(int eventId)
        {
            var ratings = await _context.Eventratings
                .Where(r => r.EventId == eventId && r.Status == "Active")
                .Select(r => r.Rating)
                .ToListAsync();

            int ratingCount = ratings.Count;

            if (ratingCount == 0)
            {
                return new
                {
                    AverageRating = 0,
                    RatingCount = 0
                };
            }

            double averageRating = (double)ratings.Average();

            return new
            {
                AverageRating = averageRating,
                RatingCount = ratingCount
            };
        }
        
        public object searchEventByContainTiTile(string searchString)
        {
            try
            {
                var listEventAfterString = new List<Event>();
                if (searchString != null || searchString != "")
                {
                    listEventAfterString = _context.Events.Include(e => e.Tickettypes).Where(x => x.EventName.Contains(searchString)).ToList();
                }
                else
                {
                    listEventAfterString = _context.Events.Include(e => e.Tickettypes).ToList();
                }
                return new
                {
                    status = 200,
                    listEventAfterString = listEventAfterString,
                };
            }
            catch
            {
                return new
                {
                    status = 400,
                };
            }
        }

        public object searchEventByFilter(string filter)
        {
            try
            {
                var filterValues = filter.Split(',').Select(f => f.Trim()).ToList();
                var listWhenFilter = new List<Event>();

                foreach (var filterValue in filterValues)
                {
                    var checkEvent = _context.Events.Where(e => e.Status == "Đã duyệt").ToList();

                    foreach (var events in checkEvent)
                    {
                        if (filterValue.Equals("Miễn phí"))
                        {
                            var checkTypeOfTicketTypeFree = _context.Tickettypes
                                .Where(x => x.Price == 0 && x.EventId == events.EventId) 
                                .FirstOrDefault();
                            if (checkTypeOfTicketTypeFree != null)
                            {
                                listWhenFilter.Add(events);
                            }
                        }
                        if (filterValue.Equals("Có phí"))
                        {
                            var checkTypeOfTicketTypePaid = _context.Tickettypes
                                .Where(x => x.Price > 0 && x.EventId == events.EventId)
                                .FirstOrDefault();
                            if (checkTypeOfTicketTypePaid != null)
                            {
                                listWhenFilter.Add(events);
                            }
                        }
                        if (filterValue.Equals("Nghệ thuật"))
                        {
                            var checkEventByCategory1 = _context.Events.Include(e => e.Tickettypes)
                                .Where(x => x.CategoryId == 1 && x.EventId == events.EventId && x.Status == "Đã duyệt")
                                .ToList();
                            foreach (var eventByCategory1 in checkEventByCategory1)
                            {
                                listWhenFilter.Add(eventByCategory1);
                            }
                        }
                        if (filterValue.Equals("Giáo dục"))
                        {
                            var checkEventByCategory2 = _context.Events.Include(e => e.Tickettypes)
                                .Where(x => x.CategoryId == 2 && x.EventId == events.EventId && x.Status == "Đã duyệt")
                                .ToList();
                            foreach (var eventByCategory2 in checkEventByCategory2)
                            {
                                listWhenFilter.Add(eventByCategory2);
                            }
                        }
                        if (filterValue.Equals("Workshop"))
                        {
                            var checkEventByCategory3 = _context.Events.Include(e => e.Tickettypes)
                                .Where(x => x.CategoryId == 3 && x.EventId == events.EventId && x.Status == "Đã duyệt")
                                .ToList();
                            foreach (var eventByCategory3 in checkEventByCategory3)
                            {
                                listWhenFilter.Add(eventByCategory3);
                            }
                        }
                        if (filterValue.Equals("Khác"))
                        {
                            var checkEventByCategory4 = _context.Events.Include(e => e.Tickettypes)
                                .Where(x => x.CategoryId == 4 && x.EventId == events.EventId && x.Status == "Đã duyệt")
                                .ToList();
                            foreach (var eventByCategory4 in checkEventByCategory4)
                            {
                                listWhenFilter.Add(eventByCategory4);
                            }
                        }
                    }
                }

                var resultFilter = listWhenFilter.GroupBy(e => e.EventId)
                    .Select(g => g.First()).OrderBy(e => e.StartTime)
                    .ToList();

                return new
                {
                    status = 200,
                    resultFilter = resultFilter,
                };
            }
            catch
            {
                return new
                {
                    status = 400,
                };
            }
        }
        public async Task<object> GetAllEventUser()
        {
            var data = _context.Events.Where(e => e.Status == "Đã duyệt")
                .Include(e => e.Category)
                .Include(e => e.Tickettypes)
                .Include(e => e.Account)
                .OrderBy(e => e.StartTime)
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

        //update for organizer
        public object AddTicketType(TicketTypeDTO ticketType)
        {
            try
            {
                var existingTicketType = _context.Tickettypes.FirstOrDefault(tt => tt.EventId == ticketType.EventId && tt.TypeName == ticketType.TypeName);
                if (existingTicketType != null)
                {
                    return new
                    {
                        message = "TicketTypeIsExist",
                        status = 400
                    };
                }
                var newTicketType = new Tickettype
                {
                    EventId = ticketType.EventId,
                    TypeName = ticketType.TypeName,
                    Price = ticketType.Price,
                    Quantity = ticketType.Quantity
                };

                _context.Tickettypes.Add(newTicketType);
                _context.SaveChanges();

                return new
                {
                    message = "Add TicketType Success",
                    status = 200,
                    newTicketType
                };
            }
            catch
            {
                return new
                {
                    message = "Add TicketType Fail",
                    status = 400
                };
            }
        }

    }
}