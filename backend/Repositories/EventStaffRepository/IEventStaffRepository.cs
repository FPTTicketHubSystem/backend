using backend.DTOs;

namespace backend.Repositories.EventStaffRepository
{
    public interface IEventStaffRepository
    {
        object RegisterStaff(EventStaff eventStaff);
        Task<object> GetStaffByEvent(int eventId);
        object ApproveStaff(int accountId, int eventId, string status);
    }
}
