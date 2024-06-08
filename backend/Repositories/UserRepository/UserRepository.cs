using backend.DTOs;
using backend.Models;
using backend.Services.OtherService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly FpttickethubContext _context;

        public UserRepository()
        {
            _context = new FpttickethubContext();
        }


        public async Task<object> ForgotPassword(string email)
        {
            var account = _context.Accounts.FirstOrDefault(x => x.Email.Equals(email));
            if (account == null)
            {
                return new
                {
                    message = "Not found account",
                    status = 400,
                };
            }
            else
            {
                var unencryptedPassword = GenerateRandomString();
                account.Password = BCrypt.Net.BCrypt.HashPassword(unencryptedPassword);
                //account.Password = GenerateRandomString();
                _context.SaveChanges();
                try
                {
                    EmailService.Instance.SendMail(account.Email, 2, account.FullName, account.Email, unencryptedPassword);
                }
                catch
                {
                    return new
                    {
                        message = "Send Mail Failly",
                        status = 400,
                    };
                }
                return new
                {
                    message = "Send Mail Change Password Successfully",
                    status = 200,
                    account,
                };
            }
        }

        public async Task<object> GetInfo(string token)
        {
            string _token = token.Split(' ')[1];
            if (_token == null)
            {
                return new
                {
                    message = "Token is wrong",
                    status = 400,
                };
            }
            var handle = new JwtSecurityTokenHandler();
            string email = handle.ReadJwtToken(_token).Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            var user = _context.Accounts.Where(x => x.Email == email).FirstOrDefault();
            if (user == null)
            {
                return new
                {
                    message = "User is not found",
                    status = 400,
                };
            }
            return new
            {
                message = "Get information success!",
                status = 200,
                data = user,
            };
        }
        public object LoginByGoogle(Register register, IConfiguration config)
        {
            var checkUser = _context.Accounts.SingleOrDefault(x => x.Email.Equals(register.Email));
            if (checkUser == null)
            {
                var newAccount = new Account();
                newAccount.FullName = register.FullName;
                newAccount.Email = register.Email;
                newAccount.CreateDate = DateTime.Now;   
                newAccount.Status = "Đang hoạt động";
                newAccount.Avatar = register.Avatar;
                newAccount.RoleId = 2;
                newAccount.Password = BCrypt.Net.BCrypt.HashPassword(GenerateRandomString());
                //newAccount.Password = GenerateRandomString();
                _context.Accounts.Add(newAccount);
                _context.SaveChanges();
                string token = "";
                var user = _context.Accounts.FirstOrDefault(x => x.Email == register.Email);
                token = CreateToken(user.Email, (int)user.RoleId, config);
                return new
                {
                    message = "Create A New Account Successfully",
                    status = 400,
                    data = user,
                    token,
                    user.RoleId,
                };
            }
            else
            {
                string token = "";
                var user = _context.Accounts.FirstOrDefault(x => x.Email == register.Email);
                token = CreateToken(user.Email, (int)user.RoleId, config);
                return new
                {
                    message = "Login success!",
                    status = 200,
                    data = user,
                    token,
                    user.RoleId,
                };
            }
        }

        public object Login(string email, string password, IConfiguration config)
        {
            string token = "";
            var user = _context.Accounts.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return new
                {
                    status = 400,
                    message = "The account is not found"
                };
            }
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return new
                {
                    message = "Password is wrong",
                    status = 400
                };
            }
            if (user.Status.Equals("Chờ xác thực"))
            {
                return new
                {
                    message = "Please check your email to confirm your account",
                    status = 400
                };
            }
            if (user.Status.Equals("Đang khóa"))
            {
                return new
                {
                    message = "Your account is blocked. Please contact admin!",
                    status = 400
                };
            }
            token = CreateToken(user.Email, (int)user.RoleId, config);
            if (user.Email == email && BCrypt.Net.BCrypt.Verify(password, user.Password) && user.Status.Equals("Đang hoạt động"))
            {
                return new
                {
                    message = "Login success!",
                    status = 200,
                    data = user,
                    token,
                    user.RoleId,
                };
            }
            return null;
        }

        public object Register(Register register)
        {
            var account = new Account();
            account.FullName = register.FullName;
            account.Email = register.Email;
            account.Phone = register.PhoneNumber;
            account.CreateDate = DateTime.Now;
            //account.Status = "Chờ xác thực";
            if (register.Status == null)
            {
                account.Status = "Chờ xác thực";
            }
            else
            {
                account.Status = register.Status;
            }
            if (register.Avatar != null)
            {
                account.Avatar = register.Avatar;
            }
            account.RoleId = 2;
            //var unencryptedPassword = GenerateRandomString();
            account.Password = BCrypt.Net.BCrypt.HashPassword(register.Password);
            //account.Password = GenerateRandomString();
            _context.Accounts.Add(account);
            _context.SaveChanges();
            if (account.Status.Equals("Chờ xác thực"))
            {
                try
                {
                    EmailService.Instance.SendMail(account.Email, 1, account.FullName, account.Email, account.Password);
                }
                catch
                {
                    return new
                    {
                        message = "Send Mail Failly",
                        status = 400,
                    };
                }
            }
            return account;

        }

        public string CreateToken(string email, int id, IConfiguration config)
        {
            string role = _context.Roles.Find(id).RoleName;
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                config.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return "bearer " + jwt;
        }

        public string GenerateRandomString()
        {
            Random random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] randomChars = new char[10];
            randomChars[0] = characters[random.Next(26)];
            for (int i = 1; i < 10; i++)
            {
                randomChars[i] = characters[random.Next(characters.Length)];
            }
            string randomString = new string(randomChars);
            return randomString;
        }

        public object UpdateUser(UpdateUser user)
        {
            try
            {
                var updateUser = _context.Accounts.SingleOrDefault(x => x.AccountId == user.AccountId);
                if (updateUser == null)
                {
                    return new
                    {
                        message = "Not Found User",
                        status = 200,
                    };
                }

                updateUser.FullName = user.FullName;
                updateUser.Phone = user.Phone;
                updateUser.BirthDay = user.BirthDay;
                updateUser.Gender = user.Gender;
                //updateUser.SchoolName = user.SchoolName;
                updateUser.Avatar = user.Avatar;
                _context.SaveChanges();
                return new
                {
                    message = "Update Successfully",
                    status = 200,
                    updateUser,
                };
            }
            catch
            {
                return new
                {
                    message = "Update Failly",
                    status = 400,
                };
            }

        }

        public async Task<object> ChangePassword(int accountId, string newPassword)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.AccountId == accountId);
            if (account == null)
            {
                return new
                {
                    message = "Not Fount Account",
                    status = 400,
                };
            }
            else
            {
                //account.Password = newPassword;
                account.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                _context.SaveChanges();
                return new
                {
                    message = "Change Password Successfully",
                    status = 200,
                    account,
                };
            }
        }

        public object ChangeAvatar(int accountId, string newAvatar)
        {
            var account = _context.Accounts.FirstOrDefault(x => x.AccountId == accountId);
            if (account == null)
            {
                return new
                {
                    message = "Not Found Account",
                    status = 400,
                };
            }
            else
            {
                account.Avatar = newAvatar;
                _context.SaveChanges();
                return new
                {
                    message = "Change Avatar Successfully",
                    status = 200,
                };
            }
        }

        public async Task<object> SearchInforByEmail(string email)
        {
            var user = _context.Accounts.SingleOrDefault(x => x.Email.Equals(email));
            if (user == null)
            {
                return new
                {
                    message = "Not Found User",
                    status = 400,
                };

            }
            else
            {
                return new
                {
                    message = "Get Infor Successfully",
                    status = 200,
                    user,
                };
            }
        }


        public object GetAllAccountUser()
        {
            var userList = _context.Accounts.Where(x => x.RoleId == 4).OrderByDescending(x => x.AccountId);
            if (userList == null)
            {
                return new
                {
                    message = "No Data to return",
                    status = 400,
                };
            }
            return new
            {
                message = "Get data successfully",
                status = 200,
                userList
            };
        }

        public object UpdateAccountUser(AccountDTO user)
        {
            try
            {
                var updateUser = _context.Accounts.SingleOrDefault(x => x.AccountId == user.AccountId);
                updateUser.FullName = user.FullName;
                updateUser.Phone = user.Phone;
                updateUser.Gender = user.Gender;
                updateUser.BirthDay = user.BirthDay;
                //updateUser.SchoolName = user.SchoolName;
                _context.SaveChanges();
                return new
                {
                    message = "Update Successfully",
                    status = 200,
                    data = updateUser,
                };
            }
            catch
            {
                return new
                {
                    message = "Update Failly",
                    status = 400,
                };
            }


        }

        public object GetAccountUserById()
        {
            throw new NotImplementedException();
        }

        public object ChangeStatusUser(int accountId, string status)
        {
            var updateStatus = _context.Accounts.SingleOrDefault(x => x.AccountId == accountId);
            if (updateStatus == null)
            {
                return new
                {
                    message = "The user doesn't exist in database",
                    status = 400
                };
            }
            else
            {
                updateStatus.Status = status;
                _context.SaveChanges();
                return new
                {
                    status = 200,
                    data = updateStatus,
                    message = "Update successfully!"
                };
            }
        }

        public DateTime GetSundayOfWeek(DateTime date)
        {
            int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)date.DayOfWeek + 7) % 7;
            return date.AddDays(daysUntilSunday);
        }

        public object GetPhoneNumberWithoutThisPhone(string phoneNumber)
        {
            var result = from account in _context.Accounts.Where(x => x.Phone != phoneNumber)
                         select new
                         {
                             phoneNumber = account.Phone,
                         };
            List<string> data = new List<string>();
            foreach (var phone in result)
            {
                data.Add(phone.phoneNumber);
            }
            return new
            {
                message = "Get successfully",
                status = 200,
                data = data,
            };
        }


    }
}
