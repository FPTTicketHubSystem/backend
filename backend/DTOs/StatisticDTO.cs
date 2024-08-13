namespace backend.DTOs
{
    public class MonthlyRevenueDTO
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class TopRatedEventDTO
    {
        public string EventName { get; set; }
        public double AverageRating { get; set; }
    }

    public class EventRevenueDTO
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string OrganizerName { get; set; }
        public string OrganizerEmail { get; set; }
        public string OrganizerPhoneNumber { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class TopParticipantsDTO
    {
        public string AccountName { get; set; }
        public int EventsParticipated { get; set; }
    }

    public class TopRevenueEventDTO
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class TopParticipantsEventDTO
    {
        public string EventName { get; set; }
        public int ParticipantsCount { get; set; }
    }
    public class MonthlyRegisteredUsersDTO
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? TotalRegisteredUsers { get; set; }
    }
}
