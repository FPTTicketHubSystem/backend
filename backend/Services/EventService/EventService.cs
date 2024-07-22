using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories.EventRepository;
using MimeKit.Encodings;
using Org.BouncyCastle.Asn1.X509;

namespace backend.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<object> GetAllEvent()
        {
            return await _eventRepository.GetAllEvent();
        }

        public async Task<object> GetAllEventAdmin()
        {
            return await _eventRepository.GetAllEventAdmin();
        }

        public object AddEvent(EventDTO newEvent)
        {
            var result = _eventRepository.AddEvent(newEvent);
            return result;
        }

        public object EditEvent(int eventId, EventDTO updatedEventDto)
        {
            var result = _eventRepository.EditEvent(eventId, updatedEventDto);
            return result;
        }

        public object GetEventById (int eventId)
        {
            var result = _eventRepository.GetEventById(eventId);
            return result;
        }

        public object GetEventByCategory (int categoryId)
        {
            var result = _eventRepository.GetEventByCategory(categoryId);
            return result;
        }

        public async Task<object> GetUpcomingEvent()
        {
            return await _eventRepository.GetUpcomingEvent();
        }

        public async Task<object> ChangeEventStatus(int eventId, string status)
        {
            return await _eventRepository.ChangeEventStatus(eventId, status);
        }

    }
}
