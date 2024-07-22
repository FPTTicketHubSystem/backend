namespace backend.Services.StatisticService
{
    public interface IStatisticService
    {
        int GetTotalParticipants(int eventId);
        int GetUserEventCount(int userId);
        decimal GetTotalIncome(int eventId);
        int GetTotalTicketsSold(int eventId);
    }
}
