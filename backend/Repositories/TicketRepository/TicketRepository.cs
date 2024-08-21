using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace backend.Repositories.TicketRepository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly FpttickethubContext _context;

        public TicketRepository(FpttickethubContext context)
        {
            _context = context;
        }

        public async Task<object> GetTicketByAccount(int accountId)
        {
            var data = _context.Tickets
                .Include(t => t.OrderDetail)
                .Where(t => t.OrderDetail.Order.AccountId == accountId && t.OrderDetail.Order.Status == "Đã thanh toán")
                .OrderBy(t => t.OrderDetail.TicketType.Event.StartTime)
                .Select(t =>
                new
                {
                    orderCode = "FTH2024" + t.OrderDetail.Order.OrderId,
                    t.TicketId,
                    t.OrderDetail.TicketType.TypeName,
                    t.OrderDetail.TicketType.Event.EventName,
                    t.OrderDetail.TicketType.Event.StartTime,
                    t.OrderDetail.TicketType.Event.EndTime,
                    t.OrderDetail.TicketType.Event.Location,
                    t.OrderDetail.TicketType.Event.Address,
                    t.IsCheckedIn,
                });
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public object GetTicketById(int ticketId, int userId)
        {
            var data = _context.Tickets
            .Include(t => t.OrderDetail).ThenInclude(od => od.Order)
                            .Where(t => t.TicketId == ticketId && t.OrderDetail.Order.Status == "Đã thanh toán" && t.OrderDetail.Order.AccountId == userId)
                            .Select(t =>
                            new
                            {
                                t.TicketId,
                                t.OrderDetail.TicketType.TypeName,
                                t.OrderDetail.TicketType.Event.EventName,
                                t.OrderDetail.TicketType.Event.StartTime,
                                t.OrderDetail.TicketType.Event.EndTime,
                                t.OrderDetail.TicketType.Event.Location,
                                t.OrderDetail.TicketType.Event.Address,
                                t.OrderDetail.TicketType.Event.ThemeImage,
                                t.OrderDetail.Order.OrderDate,
                                t.OrderDetail.Order.AccountId,
                                t.OrderDetail.Order.Account.FullName,
                                t.OrderDetail.Order.Account.Email,
                                t.OrderDetail.Order.Account.Phone,
                                t.OrderDetail.Quantity,
                                t.IsCheckedIn,
                                t.CheckInDate,
                                orderId = "FTH2024" + t.OrderDetail.Order.OrderId,
                                paymentAmount = t.OrderDetail.Order.Payments.FirstOrDefault().PaymentAmount ?? 0

                            }).FirstOrDefault();
            if (data == null)
            {
                return new
                {
                    status = 400,
                    message = "NotFound"
                };
            }
            return data;
        }


    }
}
