using backend.DTOs;

namespace backend.Services.EventStaffService
{
    public interface IEventStaffService
    {
        object RegisterStaff(EventStaff eventStaff);
        object AddStaffByEmail(string email, int eventId);
        object DeleteStaff(int staffId, int eventId);
        Task<object> GetUpcomingEventByOrganizer(int organizerId);
        Task<object> GetStaffByEvent(int eventId);
    }
}
