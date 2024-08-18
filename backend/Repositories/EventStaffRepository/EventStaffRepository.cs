using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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

        public object AddStaffByEmail(string email, int eventId)
        {
            var user = _context.Accounts.FirstOrDefault(a => a.Email == email);
            if (user == null)
            {
                return new
                {
                    message = "User not found",
                    status = 400
                };
            }
            //if (user.RoleId == 4)
            //{
            //    return new
            //    {
            //        message = "Already is staff",
            //        status = 400
            //    };
            //}
            if (user.RoleId == 1 || user.RoleId == 3)
            {
                return new
                {
                    message = "Unable to add",
                    status = 400
                };
            }
            var existingStaff = _context.Eventstaffs.FirstOrDefault(es => es.AccountId == user.AccountId && es.EventId == eventId);
            if (existingStaff != null)
            {
                return new
                {
                    message = "Staff already added for this event",
                    status = 400
                };
            }
            var newStaff = new Eventstaff
            {
                AccountId = user.AccountId,
                EventId = eventId
            };
            _context.Add(newStaff);
            newStaff.Account.RoleId = 4;
            _context.SaveChanges();

            return new
            {
                message = "Staff Added",
                status = 200,
                newStaff
            };
        }


        public object DeleteStaff(int staffId, int eventId)
        {
            var staff = _context.Eventstaffs
                .FirstOrDefault(es => es.AccountId == staffId && es.EventId == eventId);

            if (staff == null)
            {
                return new
                {
                    message = "Not Found",
                    status = 400
                };
            }
            _context.Eventstaffs.Remove(staff);
            _context.SaveChanges();

            var remainingStaffRoles = _context.Eventstaffs
                .Where(es => es.AccountId == staffId)
                .ToList();

            if (!remainingStaffRoles.Any())
            {
                var user = _context.Accounts.FirstOrDefault(a => a.AccountId == staffId);
                if (user != null && user.RoleId == 4)
                {
                    user.RoleId = 2;
                }
            }

            _context.SaveChanges();

            return new
            {
                message = "Staff removed",
                status = 200
            };
        }


        public async Task<object> GetUpcomingEventByOrganizer (int organizerId)
        {
            var data = _context.Events
                .Include(e => e.Eventstaffs)
                .Where(e => e.AccountId == organizerId && e.Status == "Đã duyệt" && e.EndTime > DateTime.UtcNow)
                .OrderBy(e => e.StartTime)
                .Select(e =>
                new
                {
                    e.EventId,
                    e.EventName,
                    e.StartTime,
                    e.EndTime,
                    e.Location,
                    e.Address,
                    numberOfStaff = e.Eventstaffs.Count
                });
            if (data == null)
            {
                return null;
            }
            return data;
        }

        public async Task<object> GetStaffByEvent(int eventId)
        {
            var data = _context.Eventstaffs
                .Include(s => s.Account)
                .Include(s => s.Event)
                .Where(s => s.EventId == eventId)
                .OrderByDescending(s => s.Account.CreateDate)
                .Select(s =>
                new
                {
                    s.AccountId,
                    s.EventId,
                    s.Account.Avatar,
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

    }
}
