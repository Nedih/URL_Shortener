using Microsoft.AspNetCore.Identity;
using URL_Shortener.BLL.Models;
using URL_Shortener.BLL.Models.Identity;

namespace URL_Shortener.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetUsers();
        IEnumerable<ProfileDTO> GetUsersList();
        Task<UserDTO> GetUser(string id);
        public UserDTO GetUserByLogin(string login);
        Task<IdentityResult> UpdateUser(string id, UserDTO userDto);
        Task<IdentityResult> UpdateUser(string id, ProfileDTO userDto);
        Task<IdentityResult> CreateUser(RegisterViewModel userDto);
        Task<IdentityResult> UpdatePassword(string id, ChangePasswordViewModel model);
        Task<IdentityResult> UpdateEmail(string currentEmail, string newEmail);
    }
}
