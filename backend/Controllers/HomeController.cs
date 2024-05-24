using backend.DTOs;
using backend.Models;
using backend.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        private readonly FpttickethubContext _db;

        public HomeController(FpttickethubContext db, IUserService userService, IConfiguration configuration)
        {
            this._db = db;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(Login login)
        {
            try
            {
                var result = _userService.Login(login.Email, login.Password, _configuration);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }



        [HttpPost("register")]
        public async Task<ActionResult> Register(Register register)
        {
            try
            {
                var result = _userService.Register(register);
                return Ok(new
                {
                    message = "Add user success!",
                    status = 200,
                    data = result
                });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("confirm")]
        public async Task<ActionResult> ConfirmAccount(string email)
        {
            try
            {
                var _user = _db.Accounts.SingleOrDefault(x => x.Email.Equals(email));
                if (_user == null)
                {
                    return NotFound();
                }
                _user.Status = "Đang hoạt động";
                //_db.Entry(_db.Accounts.FirstOrDefaultAsync(x => x.Email == email)).CurrentValues.SetValues(_user);
                _db.SaveChanges();
                return Ok(new
                {
                    status = 200,
                    message = "Confirm success"
                });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("getAllPhone")]
        public async Task<ActionResult> GetAllPhone()
        {
            var result = (from account in _db.Accounts
                          select account.Phone).ToList();
            if (result == null)
            {
                return Ok(new
                {
                    message = "No Data",
                    status = 400,
                });
            }
            return Ok(result);
        }

        [HttpPost("forgotPassword")]
        public async Task<ActionResult> ChangePassword(string email)
        {
            try
            {
                var result = _userService.ForgotPassword(email);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("loginByGoogle")]
        public async Task<ActionResult> LoginByGoogle(Register register)
        {
            try
            {
                var result = _userService.LoginByGoogle(register, _configuration);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("search")]
        public async Task<ActionResult> SearchInforByEmail(string email)
        {
            try
            {
                var result = _userService.SearchInforByEmail(email);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        //[HttpGet("weeklyActivity")]
        //public async Task<ActionResult> WeeklyActivity(int accountId)
        //{
        //    try
        //    {
        //        var result = _userService.WeekLyActivity(accountId);
        //        return Ok(result);
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}


    }
}
