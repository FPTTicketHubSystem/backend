using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories.EventRepository;

namespace backend.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }


        public object AddEvent(EventDTO newEvent)
        {
            var result = _eventRepository.AddEvent(newEvent);
            return result;
        }

    }
}
