using backend.DTOs;

namespace backend.Services.EventStaffService
{
    public interface IEventStaffService
    {
        object RegisterStaff(EventStaff eventStaff);
        Task<object> GetStaffByEvent(int eventId);
        object ApproveStaff(int accountId, int eventId, string status);
    }
}
