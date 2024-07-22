using backend.Repositories.StatisticRepository;

namespace backend.Services.StatisticService
{
    public class StatisticService : IStatisticService
    {
        private readonly IStatisticRepository _statisticRepository;
        public StatisticService(IStatisticRepository statisticRepository) { 
            _statisticRepository = statisticRepository;
        }
        public int GetTotalParticipants(int eventId)
        {
            return _statisticRepository.GetTotalParticipants(eventId);
        }

        public int GetUserEventCount(int userId)
        {
            return _statisticRepository.GetUserEventCount(userId);
        }

        public decimal GetTotalIncome(int eventId)
        {
            return _statisticRepository.GetTotalIncome(eventId);
        }

        public int GetTotalTicketsSold(int eventId)
        {
            return _statisticRepository.GetTotalTicketsSold(eventId);
        }
    }
}
