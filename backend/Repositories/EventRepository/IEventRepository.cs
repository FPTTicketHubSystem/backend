using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Repositories.EventRepository
{
    public interface IEventRepository
    {
        Task<object> GetAllEvent();
        Task<object> GetEventByAccount(int accountId);
        object AddEvent(EventDTO newEventDto);
        object GetEventForEdit(int eventId);
        object EditEvent(EventDTO updatedEventDto);
        object GetEventById(int eventId);
        object GetEventByCategory(int categoryId);
        Task<object> GetUpcomingEvent();
    }
}