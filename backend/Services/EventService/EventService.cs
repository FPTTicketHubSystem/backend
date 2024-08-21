using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories.EventRepository;
using MimeKit.Encodings;
using Org.BouncyCastle.Asn1.X509;
using static backend.Repositories.EventRepository.EventRepository;

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

        public async Task<object> ChangeEventStatus(int eventId, string status)
        {
            return await _eventRepository.ChangeEventStatus(eventId, status);
        }

        public async Task<object> GetEventByAccount(int accountId)
        {
            return await _eventRepository.GetEventByAccount(accountId);
        }

        public object GetEventForEdit(int eventId)
        {
            var result = _eventRepository.GetEventForEdit(eventId);
            return result;
        }

        //update for oganizer manage
        public async Task<object> GetTicketTypeByEvent(int eventId)
        {
            var result = _eventRepository.GetTicketTypeByEvent(eventId);
            return result;
        }
        public async Task<object> UpdateTicketQuantity(int ticketTypeId, int addQuantity)
        {
            var result = _eventRepository.UpdateTicketQuantity(ticketTypeId, addQuantity);
            return result;
        }
        public async Task<object> GetDiscountCodeByEvent(int eventId)
        {
            var result = _eventRepository.GetDiscountCodeByEvent(eventId);
            return result;
        }
        public object AddDiscountCode(DiscountCodeDTO discountcode)
        {
            var result = _eventRepository.AddDiscountCode(discountcode);
            return result;
        }
        public async Task<object> UpdateDiscountQuantity(int discountId, int addQuantity)
        {
            var result = _eventRepository.UpdateDiscountQuantity(discountId, addQuantity);
            return result;
        }

        public int GetNumberOfTicketSold(int eventId)
        {
            return _eventRepository.GetNumberOfTicketSold(eventId);
        }

        public decimal GetTotalRevenue(int eventId)
        {
            return _eventRepository.GetTotalRevenue(eventId);
        }

        public int GetActualParticipants(int eventId)
        {
            return _eventRepository.GetActualParticipants(eventId);
        }

        public async Task<List<TicketSalesPerTicketType>> GetTicketSalesPerTicketType(int eventId)
        {
            return await _eventRepository.GetTicketSalesPerTicketType(eventId);
        }

        public async Task<object> GetEventStatus(int eventId)
        {
            return await _eventRepository.GetEventStatus(eventId);
        }

        public async Task<object> GetAverageRating(int eventId)
        {
            return await _eventRepository.GetAverageRating(eventId);
        }
        
        public object searchEventByContainTiTile(string searchString)
        {
            return _eventRepository.searchEventByContainTiTile(searchString);
        }

        public object searchEventByFilter(string filter)
        {
            return _eventRepository.searchEventByFilter(filter);
        }
        public async Task<object> GetAllEventUser()
        {
            return _eventRepository.GetAllEventUser();
        }

        //update for organizer
        public object AddTicketType(TicketTypeDTO ticketType)
        {
            return _eventRepository.AddTicketType(ticketType);
        }
    }
}