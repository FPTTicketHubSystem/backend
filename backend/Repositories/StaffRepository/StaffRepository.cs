using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace backend.Repositories.StaffRepository
{
    public class StaffRepository : IStaffRepository
    {
        private readonly FpttickethubContext _context;

        public StaffRepository(FpttickethubContext context)
        {
            _context = context;
        }

        public object CheckInTicket(int ticketId, int staffId)
        {
            var data = _context.Tickets
                .Include(t => t.OrderDetail)
                .Where(t => t.TicketId == ticketId && t.OrderDetail.Order.Status == "Đã thanh toán" && t.OrderDetail.TicketType.Event.Eventstaffs.Any(es => es.AccountId == staffId))
                .Select(t => new
                {
                    ticketCode = t.TicketId,
                    ticketType = t.OrderDetail.TicketType.TypeName,
                    eventName = t.OrderDetail.TicketType.Event.EventName,
                    eventStartTime = t.OrderDetail.TicketType.Event.StartTime,
                    eventEndTime = t.OrderDetail.TicketType.Event.EndTime,
                    eventLocation = t.OrderDetail.TicketType.Event.Location,
                    eventAddress = t.OrderDetail.TicketType.Event.Address,
                    eventThemeImage = t.OrderDetail.TicketType.Event.ThemeImage,
                    ticketQuantity = t.OrderDetail.Quantity,
                    orderId = t.OrderDetail.OrderId,
                    orderStatus = t.OrderDetail.Order.Status,
                    isCheckedIn = t.IsCheckedIn,
                    checkInDate = t.CheckInDate
                }).SingleOrDefault();

            if (data == null)
            {
                return new
                {
                    status = 400,
                    message = "NotFoundTicket"
                };
            }

            var now = DateTime.UtcNow;
            if (data.eventStartTime.HasValue && (now < data.eventStartTime.Value.AddHours(-2)))
            {
                return new
                {
                    status = 400,
                    message = "EarlyToCheckin"
                };
            }

            if (data.eventEndTime.HasValue && (now > data.eventStartTime.Value.AddMinutes(45)))
            {
                return new
                {
                    status = 400,
                    message = "CheckinTimeIsOver"
                };
            }

            if (data.isCheckedIn == true && data.checkInDate != null)
            {
                return new
                {
                    status = 200,
                    message = "AlreadyCheckedIn",
                    data
                };
            }

            var checkInResult = ChangeCheckInStatus(ticketId, true, DateTime.UtcNow);


            if (checkInResult == null)
            {
                return new
                {
                    status = 500,
                    message = "ErrorUpdatingCheckInStatus"
                };
            }

            data = _context.Tickets
                .Include(t => t.OrderDetail)
                .Where(t => t.TicketId == ticketId)
                .Select(t => new
                {
                    ticketCode = t.TicketId,
                    ticketType = t.OrderDetail.TicketType.TypeName,
                    eventName = t.OrderDetail.TicketType.Event.EventName,
                    eventStartTime = t.OrderDetail.TicketType.Event.StartTime,
                    eventEndTime = t.OrderDetail.TicketType.Event.EndTime,
                    eventLocation = t.OrderDetail.TicketType.Event.Location,
                    eventAddress = t.OrderDetail.TicketType.Event.Address,
                    eventThemeImage = t.OrderDetail.TicketType.Event.ThemeImage,
                    ticketQuantity = t.OrderDetail.Quantity,
                    orderId = t.OrderDetail.OrderId,
                    orderStatus = t.OrderDetail.Order.Status,
                    isCheckedIn = t.IsCheckedIn,
                    checkInDate = t.CheckInDate
                }).SingleOrDefault();

            if (data == null)
            {
                return new
                {
                    status = 400,
                    message = "NotFound"
                };
            }

            return new
            {
                status = 200,
                message = "TicketFoundAndCheckedIn",
                data
            };
        }

        public object ChangeCheckInStatus(int ticketId, bool isCheckIn, DateTime checkedInDate)
        {
            var ticketCheckedIn = _context.Tickets.Include(t => t.OrderDetail).SingleOrDefault(t => t.TicketId == ticketId);
            if (ticketCheckedIn == null)
            {
                return null;
            }
            else
            {
                ticketCheckedIn.IsCheckedIn = isCheckIn;
                ticketCheckedIn.CheckInDate = checkedInDate;
                _context.SaveChanges();
                return ticketCheckedIn;
            }
        }

        public async Task<object> GetCheckinHistoryByEvent(int staffId)
        {
            var today = DateTime.UtcNow.Date;

            var data = _context.Tickets
                .Include(t => t.OrderDetail)
                .ThenInclude(od => od.TicketType)
                .ThenInclude(tt => tt.Event)
                .Where(t => t.IsCheckedIn == true && t.CheckInDate.HasValue && t.CheckInDate.Value.Date == today
                            && t.OrderDetail.TicketType.Event.Eventstaffs.Any(es => es.AccountId == staffId))
                .GroupBy(t => new
                {
                    t.OrderDetail.TicketType.Event.EventId,
                    t.OrderDetail.TicketType.Event.EventName,
                    t.OrderDetail.TicketType.Event.StartTime
                })
                .OrderByDescending(g => g.Key.StartTime)
                .Select(g => new
                {
                    EventId = g.Key.EventId,
                    EventName = g.Key.EventName,
                    EventDate = g.Key.StartTime,
                    Checkins = g.Select(t => new
                    {
                        t.TicketId,
                        t.CheckInDate,
                        t.OrderDetail.TicketType.TypeName,
                        t.OrderDetail.Order.Account.FullName,
                        t.OrderDetail.Order.Account.Email,
                        t.OrderDetail.Order.Account.Phone,
                        t.OrderDetail.Quantity
                    }).ToList()
                }).ToList();

            return data;
        }

        public async Task<object> GetEventByStaff(int staffId)
        {
            var data = _context.Events
                .Include(e => e.Eventstaffs)
                .Where(e => e.Eventstaffs.Any(es => es.AccountId == staffId))
                .OrderBy(e => e.StartTime)
                .Select(e => new
                {
                    e.EventId,
                    e.EventName,
                    e.StartTime,
                });
            if (data == null)
            {
                return null;
            }
            else { return data; }
        }
    }
}