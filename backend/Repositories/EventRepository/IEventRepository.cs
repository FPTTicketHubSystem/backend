using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Repositories.EventRepository
{
    public interface IEventRepository
    {
        object AddEvent(EventDTO newEventDto);

    }
}