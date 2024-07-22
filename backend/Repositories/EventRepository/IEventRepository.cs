using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    }
}