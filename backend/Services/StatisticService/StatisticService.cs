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

        public async Task<decimal> GetTotalRevenue()
        {
            return await _statisticRepository.GetTotalRevenue();
        }

        public async Task<int> GetTotalParticipants()
        {
            return await _statisticRepository.GetTotalParticipants();
        }
        public async Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenue()
        {
            return await _statisticRepository.GetMonthlyRevenue();
        }
        public async Task<IEnumerable<MonthlyParticipantsDto>> GetMonthlyParticipants()
        {
            return await _statisticRepository.GetMonthlyParticipants();
        }
        public async Task<IEnumerable<EventRatingDto>> GetTopRatedEvents()
        {
            return await _statisticRepository.GetTopRatedEvents();
        }

        public async Task<IEnumerable<EventRevenueDto>> GetTopRevenueEvents()
        {
            return await _statisticRepository.GetTopRevenueEvents();
        }

        public async Task<IEnumerable<EventParticipantsDto>> GetTopParticipantsEvents()
        {
            return await _statisticRepository.GetTopParticipantsEvents();
        }

        public async Task<byte[]> GenerateEventStatisticsReport()
        {
            return await _statisticRepository.GenerateEventStatisticsReport();
        }
    }
}
