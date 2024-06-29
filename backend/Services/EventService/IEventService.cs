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
        object EditEvent(int eventId, EventDTO updatedEventDto);
        object GetEventById(int eventId);
        object GetEventByCategory(int categoryId);

        Task<object> GetUpcomingEvent();
    }
}
