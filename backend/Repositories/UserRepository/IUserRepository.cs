using backend.DTOs;
using backend.Models;
using Microsoft.Win32;

namespace backend.Repositories.UserRepository
{
    public interface IUserRepository
    {
        object Login(string email, string password, IConfiguration config);
        object Register(Register register);
        Task<object> ForgotPassword(string email);
        Task<object> GetInfo(string token);
        object UpdateUser(UpdateUser user);
        object LoginByGoogle(Register register, IConfiguration config);
        Task<object> ChangePassword(int accountId, string newPassword);
        object ChangeAvatar(int accountId, string newAvatar);

        public Task<object> SearchInforByEmail(string email);

        public object GetAllAccountUser();
        public object UpdateAccountUser(AccountDTO user);
        public object GetAccountUserById();
        object ChangeStatusUser(int accountId, string status);

        public object GetPhoneNumberWithoutThisPhone(string phoneNumber);

        Task<object> ChangeAccountRole(int accountId, int newRoleId);

    }
}
