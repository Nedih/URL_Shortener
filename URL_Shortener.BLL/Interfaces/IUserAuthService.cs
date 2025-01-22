using Microsoft.AspNetCore.Identity;
using URL_Shortener.BLL.Models.Identity;
using URL_Shortener.DAL.Entities;

namespace URL_Shortener.BLL.Interfaces
{
    public interface IUserAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        Task<IdentityResult> ValidateUserAsync(LoginViewModel model);
        Task<List<string>> GetUserClaimsAsync(LoginViewModel user);
    }
}
