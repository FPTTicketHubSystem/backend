using backend.DTOs;
using backend.Models;
using backend.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        private readonly FpttickethubContext _db;

        public UserController(FpttickethubContext db, IUserService userService, IConfiguration configuration)
        {
            this._db = db;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet("info")]
        public async Task<ActionResult> GetInfo(string token)
        {
            try
            {
                if (token == "")
                {
                    return BadRequest();
                }
                return Ok(_userService.GetInfo(token));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("changePassword")]
        public async Task<ActionResult> ChangePassword(int accountId, string newPassword)
        {
            try
            {
                var result = _userService.ChangePassword(accountId, newPassword);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("updateUser")]
        public async Task<ActionResult> UpdateUser(UpdateUser user)
        {
            try
            {
                var result = _userService.UpdateUser(user);
                return Ok(result);
            }
            catch { return BadRequest(); }
        }

        [HttpPost("ChangeStatusUser")]
        public async Task<ActionResult> ChangeStatusUser(int accountId, string newStatus)
        {
            try
            {
                var result = _userService.ChangeStatusUser(accountId, newStatus);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("getPhoneWithoutThisPhone")]
        public async Task<ActionResult> GetPhoneWithoutThisPhone(string phone)
        {
            try
            {
                var result = _userService.GetPhoneNumberWithoutThisPhone(phone);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
