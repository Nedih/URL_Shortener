using Microsoft.AspNetCore.Identity;
using URL_Shortener.BLL.Models.Identity;

namespace URL_Shortener.BLL.Interfaces
{
    public interface IUserAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        Task<IdentityResult> ValidateUserAsync(LoginViewModel model);
    }
}
