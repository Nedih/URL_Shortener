using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models.Identity;
using URL_Shortener.DAL.Entities;

namespace URL_Shortener.BLL.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly UserManager<UserAccount> _userManager;
        //private readonly RoleManager<UserAccount> _roleManager;
        private readonly IConfiguration _configuration;
        private UserAccount? _user;

        public UserAuthService(UserManager<UserAccount> userManager, /*RoleManager<UserAccount> roleManager,*/ IConfiguration configuration)
        {
            _userManager = userManager;
            //_roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<List<string>> GetUserClaimsAsync(LoginViewModel userModel)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {
            UserAccount user = new UserAccount
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Errors.Count() == 0)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            return result;
        }

        public async Task<IdentityResult> ValidateUserAsync(LoginViewModel model)
        {
            _user = await _userManager.FindByNameAsync(model.Email);
            if (_user == null)
                return IdentityResult.Failed(new IdentityError { Description = "There is no account with such email", Code = "WrongEmail" });
            if (!await _userManager.CheckPasswordAsync(_user, model.Password))
                return IdentityResult.Failed(new IdentityError { Description = "You entered wrong password", Code = "WrongPassword" });
            if (await _userManager.IsLockedOutAsync(_user))
                return IdentityResult.Failed(new IdentityError { Description = "Your account is locked", Code = "UserLocked" });
            return IdentityResult.Success;
        }
    }
}
