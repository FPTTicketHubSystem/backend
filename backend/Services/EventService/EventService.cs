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

        public async Task<object> GetEventByAccount(int accountId)
        {
            return await _eventRepository.GetEventByAccount(accountId);
        }

        public object AddEvent(EventDTO newEvent)
        {
            var result = _eventRepository.AddEvent(newEvent);
            return result;
        }

        public object GetEventForEdit(int eventId)
        {
            var result = _eventRepository.GetEventForEdit(eventId);
            return result;
        }

        public object EditEvent(EventDTO updatedEventDto)
        {
            var result = _eventRepository.EditEvent(updatedEventDto);
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

    }
}
