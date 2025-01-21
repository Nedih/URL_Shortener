using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models.Identity;
using URL_Shortener.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace URL_Shortener.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserAuthService _userAccountService;

        public AuthController(IUserAuthService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        [Authorize]
        [HttpPost("sign-out")]
        public async Task<IActionResult> Logout()
        {
            var user = User.Identity;
            if (user is not null && user.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok("You are not authorized now");
            }
            return StatusCode(401);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterViewModel userRegistration)
        {

            var userResult = await _userAccountService.RegisterUserAsync(userRegistration);
            return !userResult.Succeeded ? new BadRequestObjectResult(userResult) : StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginViewModel user)
        {
            var loginResult = await _userAccountService.ValidateUserAsync(user);
            return !loginResult.Succeeded
                ? Unauthorized(loginResult.Errors)
                : Ok();
        }
    }
}
