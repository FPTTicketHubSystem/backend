using backend.Models;

namespace backend.Repositories.StatisticRepository
{
    public interface IStatisticRepository
    {
        Task<decimal> GetTotalRevenue();
        Task<int> GetTotalParticipants();
        Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenue();
        Task<IEnumerable<MonthlyParticipantsDto>> GetMonthlyParticipants();
        Task<IEnumerable<EventRatingDto>> GetTopRatedEvents();
        Task<IEnumerable<EventRevenueDto>> GetTopRevenueEvents();
        Task<IEnumerable<EventParticipantsDto>> GetTopParticipantsEvents();
        Task<byte[]> GenerateEventStatisticsReport();
    }
}
