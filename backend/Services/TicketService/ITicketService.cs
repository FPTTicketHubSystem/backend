namespace backend.Services.TicketService
{
    public interface ITicketService
    {
        Task<object> GetTicketByAccount(int accountId);
        object GetTicketById(int ticketId);
    }
}
