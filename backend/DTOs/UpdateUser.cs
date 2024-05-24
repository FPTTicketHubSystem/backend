namespace backend.DTOs
{
    public class UpdateUser
    {
        public int AccountId { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Gender { get; set; }
    }
}
