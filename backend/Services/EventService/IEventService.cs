using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Services.EventService
{
    public interface IEventService
    {
        Task<object> GetAllEvent();
        Task<object> GetEventByAccount(int accountId);
        object AddEvent(EventDTO newEvent);
        object GetEventForEdit(int eventId);
        object EditEvent(EventDTO updatedEventDto);
        object GetEventById(int eventId);
        object GetEventByCategory(int categoryId);

        Task<object> GetUpcomingEvent();
    }
}
