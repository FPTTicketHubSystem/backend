using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.EventStaffRepository
{
    public class EventStaffRepository : IEventStaffRepository
    {
        private readonly FpttickethubContext _context;

        public EventStaffRepository()
        {
            _context = new FpttickethubContext();
        }

        public object RegisterStaff (EventStaff eventStaff)
        {
            var staff = new Eventstaff();
            staff.AccountId = eventStaff.AccountId;
            staff.EventId = eventStaff.EventId;
            staff.Status = "Chờ duyệt";
            _context.Eventstaffs.Add(staff);
            _context.SaveChanges();
            return staff;
        }

        public async Task<object> GetStaffByEvent(int eventId)
        {
            var data = _context.Eventstaffs
                .Include(s => s.Account)
                .Include(s => s.Event)
                .Where(s => s.EventId == eventId)
                .Select(s =>
                new
                {
                    s.AccountId,
                    s.EventId,
                    s.Status,
                    s.Event.EventName,
                    s.Account.Email,
                    s.Account.FullName,
                    s.Account.Phone,
                });
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public object ApproveStaff (int accountId, int eventId, string status)
        {
            var staff = _context.Eventstaffs.SingleOrDefault(s => s.AccountId == accountId && s.EventId == eventId);
            if (staff == null)
            {
                return new
                {
                    message = "NotFound",
                    status = 400
                };
            }
            else
            {
                staff.Status = status;
                _context.SaveChanges();
                return new
                {
                    message = "Updated",
                    status = 200,
                    staff
                };
            }
        }
    }
}
