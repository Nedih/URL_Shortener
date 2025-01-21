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
        [HttpGet("users-list")]
        public IEnumerable<ProfileDTO> GetList()
        {
            return _userService.GetUsersList();
        }
    }
}
