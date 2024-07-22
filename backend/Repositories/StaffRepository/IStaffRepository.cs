namespace backend.Repositories.StaffRepository
{
    public interface IStaffRepository
    {
        object CheckInTicket(int ticketId, int staffId);
        Task<object> GetCheckinHistoryByEvent(int eventId, int staffId);
        Task<object> GetEventByStaff(int staffId);
    }
}
