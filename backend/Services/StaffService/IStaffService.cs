namespace backend.Services.StaffService
{
    public interface IStaffService
    {
        object CheckInTicket(int ticketId, int staffId);
        Task<object> GetCheckinHistoryByEvent(int eventId, int staffId);
        Task<object> GetEventByStaff(int staffId);
    }
}
