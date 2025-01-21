using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models;
using URL_Shortener.BLL.Models.Identity;
using System.Security.Claims;

namespace URL_Shortener.Api.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public IEnumerable<UserDTO> Get()
        {
            return _userService.GetUsers();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetUser(id);
            if (user == null)
                return NotFound("No such user with this email");
            return Ok(user);
        }


        [Authorize]
        [HttpGet("profile/{login}")]
        public IActionResult GetUserProfile(string? login)
        {
            string? userLogin = User.FindFirstValue(ClaimTypes.Name);
            login ??= userLogin;
            if (string.IsNullOrEmpty(login))
                return BadRequest("Id is empty");
            var profile = (ProfileDTO)_userService.GetUserByLogin(login);
            if (profile == null)
                return NotFound("No such user with this id");
            return Ok(profile);
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetOwnProfile()
        {
            string? login = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(login))
                return BadRequest("Id is empty");
            var profile = (ProfileDTO)_userService.GetUserByLogin(login);
            if (profile == null)
                return NotFound("No such user with this id");
            return Ok(profile);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IdentityResult> UpdateUserProfile([FromBody] ProfileDTO user)
        {
            string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _userService.UpdateUser(id, user);
        }

        [Authorize]
        [HttpPut("email")]
        public async Task<IdentityResult> UpdateEmail([FromBody] string email)
        {
            string? currentEmail = User.FindFirstValue(ClaimTypes.Email);
            return await _userService.UpdateEmail(currentEmail, email);
        }

        [Authorize]
        [HttpPut("password")]
        public async Task<IdentityResult> UpdatePassword([FromBody] ChangePasswordViewModel model)
        {
            string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _userService.UpdatePassword(id, model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IdentityResult> Create([FromBody]RegisterViewModel user)
        {

            return await _userService.CreateUser(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IdentityResult> Update(string id, [FromBody]UserDTO user)
        {

            return await _userService.UpdateUser(id, user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("lock")]
        public async Task<IdentityResult> Lock(string id)
        {

            return await _userService.LockUser(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("unlock")]
        public async Task<IdentityResult> Unlock(string id)
        {

            return await _userService.UnLockUser(id);
        }

        [Authorize]
        [HttpGet("users-list")]
        public IEnumerable<ProfileDTO> GetList()
        {
            return _userService.GetUsersList();
        }
    }
}
