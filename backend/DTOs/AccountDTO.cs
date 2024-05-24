namespace backend.DTOs
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }
        public string? FullName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public string? Gender { get; set; }
        public decimal? Gold { get; set; }

    }

    public class MonthData
    {
        public int Month { get; set; }
        public int Value { get; set; }
    }

    public class DayData
    {
        public int Day { get; set; }
        public int Value { get; set; }
    }
}
