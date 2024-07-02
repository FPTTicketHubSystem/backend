using backend.Models;

namespace backend.Repositories.StatisticRepository
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly FpttickethubContext _context;
        public StatisticRepository(FpttickethubContext context)
        {
            _context = context;
        }
        public int GetTotalParticipants(int eventId)
        {
            return _context.Orders
                           .Where(o => o.Orderdetails.Any(od => od.TicketType.EventId == eventId))
                           .Select(o => o.AccountId)
                           .Distinct()
                           .Count();
        }

        public int GetUserEventCount(int userId)
        {
            return _context.Orders
                           .Where(o => o.AccountId == userId)
                           .SelectMany(o => o.Orderdetails)
                           .Select(od => od.TicketType.EventId)
                           .Distinct()
                           .Count();
        }

        public decimal GetTotalIncome(int eventId)
        {
            return _context.Orderdetails
                           .Where(od => od.TicketType.EventId == eventId)
                           .Sum(od => od.Subtotal) ?? 0;
        }

        public int GetTotalTicketsSold(int eventId)
        {
            return _context.Orderdetails
                           .Where(od => od.TicketType.EventId == eventId)
                           .Sum(od => od.Quantity) ?? 0;
        }
    }
}
