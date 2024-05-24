using backend.DTOs;

namespace backend.Services.UserService
{
    public interface IUserService
    {
        object Login(string email, string password, IConfiguration config);
        object Register(Register register);
        Task<object> ForgotPassword(string email);
        Task<object> GetInfo(string token);
        object UpdateUser(UpdateUser user);
        Task<object> ChangePassword(int accountId, string newPassword);
        object LoginByGoogle(Register register, IConfiguration config);
        object ChangeAvatar(int accountId, string newAvatar);
        public Task<object> SearchInforByEmail(string email);

        public object GetAllAccountUser();
        public object UpdateAccountUser(AccountDTO user);
        public object GetAccountUserById();
        object ChangeStatusUser(int accountId, string status);

        public object GetPhoneNumberWithoutThisPhone(string phoneNumber);
    }
}
