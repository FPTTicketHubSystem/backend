using backend.DTOs;
using backend.Repositories.UserRepository;

namespace backend.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }



        public object ChangeAvatar(int accountId, string newAvatar)
        {
            return _userRepository.ChangeAvatar(accountId, newAvatar);
        }

        public Task<object> ChangePassword(int accountId, string newPassword)
        {
            return _userRepository.ChangePassword(accountId, newPassword);
        }
        public Task<object> ForgotPassword(string email)
        {
            return _userRepository.ForgotPassword(email);
        }

        public async Task<object> GetInfo(string token)
        {
            return _userRepository.GetInfo(token);
        }

        public object Login(string email, string password, IConfiguration config)
        {
            return _userRepository.Login(email, password, config);
        }

        public object LoginByGoogle(Register register, IConfiguration config)
        {
            return _userRepository.LoginByGoogle(register, config);
        }

        public object Register(Register register)
        {
            return _userRepository.Register(register);
        }

        public Task<object> SearchInforByEmail(string email)
        {
            return _userRepository.SearchInforByEmail(email);
        }

        public object UpdateUser(UpdateUser user)
        {
            return _userRepository.UpdateUser(user);
        }


        public object GetAllAccountUser()
        {
            return _userRepository.GetAllAccountUser();
        }

        public object UpdateAccountUser(AccountDTO user)
        {
            return _userRepository.UpdateAccountUser(user);
        }

        public object GetAccountUserById()
        {
            throw new NotImplementedException();
        }

        public object ChangeStatusUser(int accountId, string status)
        {
            return _userRepository.ChangeStatusUser(accountId, status);
        }


        public object GetPhoneNumberWithoutThisPhone(string phoneNumber)
        {
            return _userRepository.GetPhoneNumberWithoutThisPhone(phoneNumber);
        }

    }
}
