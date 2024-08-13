using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static backend.Repositories.EventRepository.EventRepository;

namespace backend.Repositories.EventRepository
{
    public interface IEventRepository
    {
        Task<object> GetAllEvent();
        Task<object> GetAllEventAdmin();
        object AddEvent(EventDTO newEventDto);
        object EditEvent(EventDTO updatedEventDto);
        object GetEventById(int eventId);
        object GetEventByCategory(int categoryId);
        Task<object> GetUpcomingEvent();
        Task<object> ChangeEventStatus(int eventId, string status);
        object GetEventForEdit(int eventId);
        Task<object> GetEventByAccount(int accountId);
        //update for event organizer manage
        Task<object> GetTicketTypeByEvent(int eventId);
        Task<object> UpdateTicketQuantity(int ticketTypeId, int addQuantity);
        Task<object> GetDiscountCodeByEvent(int eventId);
        object AddDiscountCode(DiscountCodeDTO discountcode);
        Task<object> UpdateDiscountQuantity(int discountId, int addQuantity);
        int GetNumberOfTicketSold(int eventId);
        decimal GetTotalRevenue(int eventId);
        int GetActualParticipants(int eventId);
        Task<List<TicketSalesPerTicketType>> GetTicketSalesPerTicketType(int eventId);
        Task<object> GetEventStatus(int eventId);
        Task<object> GetAverageRating(int eventId);
        object searchEventByContainTiTile(string searchString);
        object searchEventByFilter(string filter);
        Task<object> GetAllEventUser();
    }
}