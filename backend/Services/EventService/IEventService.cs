using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

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
    }
}