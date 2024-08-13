using backend.DTOs;
using backend.Models;
using backend.Repositories.NewsRepository;
using backend.Repositories.StatisticRepository;

namespace backend.Services.StatisticService
{
    public class StatisticService : IStatisticService
    {
        private readonly IStatisticRepository _statisticRepository;
        public StatisticService(IStatisticRepository statisticRepository)
        {
            _statisticRepository = statisticRepository;
        }
        public async Task<IEnumerable<MonthlyRevenueDTO>> GetMonthlyRevenue()
        {
            return await _statisticRepository.GetMonthlyRevenue();
        }

        public async Task<IEnumerable<MonthlyRegisteredUsersDTO>> GetMonthlyActiveUsers()
        {
            return await _statisticRepository.GetMonthlyActiveUsers();
        }

        public async Task<IEnumerable<TopRatedEventDTO>> GetTopRatedEvents()
        {
            return await _statisticRepository.GetTopRatedEvents();
        }

        public async Task<IEnumerable<EventRevenueDTO>> GetEventRevenue()
        {
            return await _statisticRepository.GetEventRevenue();

        }
        public async Task<IEnumerable<TopParticipantsDTO>> GetTopParticipants()
        {
            return await _statisticRepository.GetTopParticipants();
        }
        public async Task<IEnumerable<TopRevenueEventDTO>> GetTopRevenueEvents()
        {
            return await _statisticRepository.GetTopRevenueEvents();
        }
        public async Task<IEnumerable<TopParticipantsEventDTO>> GetTopParticipantsEvents()
        {
            return await _statisticRepository.GetTopParticipantsEvents();
        }
        public async Task<byte[]> GenerateEventStatisticsReport()
        {
            return await _statisticRepository.GenerateEventStatisticsReport();
        }
    }
}
