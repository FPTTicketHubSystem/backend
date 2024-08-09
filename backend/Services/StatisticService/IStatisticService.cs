using backend.DTOs;
using backend.Models;

namespace backend.Services.StatisticService
{
    public interface IStatisticService
    {
        Task<IEnumerable<MonthlyRevenueDTO>> GetMonthlyRevenue();
        Task<IEnumerable<MonthlyRegisteredUsersDTO>> GetMonthlyActiveUsers();
        Task<IEnumerable<TopRatedEventDTO>> GetTopRatedEvents();
        Task<IEnumerable<EventRevenueDTO>> GetEventRevenue();
        Task<IEnumerable<TopParticipantsDTO>> GetTopParticipants();
        Task<IEnumerable<TopRevenueEventDTO>> GetTopRevenueEvents();
        Task<IEnumerable<TopParticipantsEventDTO>> GetTopParticipantsEvents();
        Task<byte[]> GenerateEventStatisticsReport();
    }
}
