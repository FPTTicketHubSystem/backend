using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using static backend.Repositories.EventRepository.EventRepository;

namespace backend.Services.EventService
{
    public interface IEventService
    {
        Task<object> GetAllEvent();
        Task<object> GetAllEventAdmin();
        object AddEvent(EventDTO newEvent);
        object EditEvent(EventDTO updatedEventDto);
        object GetEventById(int eventId);
        object GetEventByCategory(int categoryId);
        Task<object> GetUpcomingEvent();
        Task<object> ChangeEventStatus(int eventId, string status);
        Task<object> GetEventByAccount(int accountId);
        object GetEventForEdit(int eventId);
        //update for organizer manage
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