using backend.DTOs;

namespace backend.Repositories.EventStaffRepository
{
    public interface IEventStaffRepository
    {
        object RegisterStaff(EventStaff eventStaff);
        Task<object> GetUpcomingEventByOrganizer(int organizerId);
        Task<object> GetStaffByEvent(int eventId);
        object AddStaffByEmail(string email, int eventId);
        object DeleteStaff(int staffId, int eventId);
    }
}
