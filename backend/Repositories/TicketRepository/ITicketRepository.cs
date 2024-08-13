namespace backend.Repositories.TicketRepository
{
    public interface ITicketRepository
    {
        Task<object> GetTicketByAccount(int accountId);
        object GetTicketById(int ticketId, int userId);
    }
}
