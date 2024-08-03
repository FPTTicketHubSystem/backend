using backend.DTOs;
using backend.Repositories.EventRepository;
using backend.Repositories.EventStaffRepository;

namespace backend.Services.EventStaffService
{
    public class EventStaffService : IEventStaffService
    {
        private readonly IEventStaffRepository _eventStaffRepository;

        public EventStaffService(IEventStaffRepository eventStaffRepository)
        {
            _eventStaffRepository = eventStaffRepository;
        }

        public object RegisterStaff(EventStaff eventStaff)
        {
            return _eventStaffRepository.RegisterStaff(eventStaff);
        }

        public object AddStaffByEmail(string email, int eventId)
        {
            return _eventStaffRepository.AddStaffByEmail(email, eventId);
        }

        public object DeleteStaff(int staffId, int eventId)
        {
            return _eventStaffRepository.DeleteStaff(staffId, eventId);
        }

        public Task<object> GetUpcomingEventByOrganizer(int organizerId)
        {
            return _eventStaffRepository.GetUpcomingEventByOrganizer(organizerId);
        }

        public async Task<object> GetStaffByEvent(int eventId)
        {
            return _eventStaffRepository.GetStaffByEvent(eventId);
        }


    }
}
