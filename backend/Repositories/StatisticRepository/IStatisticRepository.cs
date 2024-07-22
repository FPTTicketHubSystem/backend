namespace backend.Repositories.StatisticRepository
{
    public interface IStatisticRepository
    {
        int GetTotalParticipants(int eventId);
        int GetUserEventCount(int userId);
        decimal GetTotalIncome(int eventId);
        int GetTotalTicketsSold(int eventId);
    }
}
