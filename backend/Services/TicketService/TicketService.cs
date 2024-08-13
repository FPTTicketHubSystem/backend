using backend.Repositories.NewsRepository;
using backend.Repositories.TicketRepository;

namespace backend.Services.TicketService
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService (ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        public async Task<object> GetTicketByAccount(int accountId)
        {
            return await _ticketRepository.GetTicketByAccount(accountId);
        }
        public object GetTicketById(int ticketId, int userId)
        {
            var result = _ticketRepository.GetTicketById(ticketId, userId);
            return result;
        }

    }

}
