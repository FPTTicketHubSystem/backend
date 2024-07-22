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
        public async Task<object> GetStaffByEvent(int eventId)
        {
            return _eventStaffRepository.GetStaffByEvent(eventId);
        }

        public object ApproveStaff(int accountId, int eventId, string status)
        {
            return _eventStaffRepository.ApproveStaff(accountId, eventId, status);
        }
    }
}
