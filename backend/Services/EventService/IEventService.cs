using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Services.EventService
{
    public interface IEventService
    {
        object AddEvent(EventDTO newEvent);

    }
}
