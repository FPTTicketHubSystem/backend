namespace backend.DTOs
{
    public class Register
    {
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Avatar { get; set; }
        public string? Status { get; set; }
    }
}
