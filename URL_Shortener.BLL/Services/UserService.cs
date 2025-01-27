using AutoMapper;
using Microsoft.AspNetCore.Identity;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models;
using URL_Shortener.BLL.Models.Identity;
using URL_Shortener.DAL.Entities;
using URL_Shortener.DAL.Interfaces;

namespace URL_Shortener.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserAccount> userManager;

        private readonly IRepository<UserAccount> _repo;
        private readonly IMapper _mapper;

        public UserService(IRepository<UserAccount> repo, UserManager<UserAccount> userManager, AutoMapper.IMapper mapper)
        {
            _repo = repo;
            this.userManager = userManager;
            _mapper = mapper;
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            return _mapper.Map<List<UserDTO>>(userManager.Users.ToList());
        }

        public IEnumerable<ProfileDTO> GetUsersList()
        {
            var userModels = _mapper.Map<List<UserDTO>>(userManager.Users.ToList());
            var profileModels = new List<ProfileDTO>();
            foreach (var user in userModels)
                profileModels.Add((ProfileDTO)user);
            return profileModels;
        }

        public async Task<UserDTO> GetUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }
            else return new UserDTO();
        }

        public UserDTO GetUserByLogin(string login)
        {
            var user = _repo.FirstOrDefault(x => x.UserName == login);
            if (user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }
            else return new UserDTO();
        }

        public async Task<IdentityResult> UpdateUser(string id, UserDTO userDto)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "There is no such user.", Code = "WrongID" });
            return await Update(user, userDto);
        }

        public async Task<IdentityResult> UpdateUser(string id, ProfileDTO profile)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "There is no such user.", Code = "WrongID" });

            return await Update(user, new UserDTO(user.Id, user.Email!, profile));
        }

        public async Task<IdentityResult> Update(UserAccount user, UserDTO userDto)
        {
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "There is no such user.", Code = "WrongID" });

            _mapper.Map(userDto, user);

            IdentityResult result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
                await _repo.SaveAsync();

            return result;
        }

        public async Task<IdentityResult> UpdatePassword(string id, ChangePasswordViewModel model)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "There is no user with this Email.", Code = "WrongEmail" });
            IdentityResult result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            return result;
        }

        public async Task<IdentityResult> UpdateEmail(string currentEmail, string newEmail)
        {

            var user = await userManager.FindByEmailAsync(currentEmail);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "There is no user with this Email.", Code = "WrongEmail" });
            var token = await userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            IdentityResult result = await userManager.ChangeEmailAsync(user, newEmail, token);
            if (result.Succeeded)
            {
                await userManager.SetUserNameAsync(user, newEmail);
                await userManager.UpdateNormalizedUserNameAsync(user);
            }
            return result;
        }

        public async Task<IdentityResult> CreateUser(RegisterViewModel userDto)
        {
            UserAccount? user = await userManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = _mapper.Map<UserAccount>(userDto);
                var result = await userManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() == 0)
                {
                    await userManager.AddToRoleAsync(user, "User");
                    await _repo.SaveAsync();
                }
                return result;
            }
            else
            {
                return IdentityResult.Failed(new IdentityError { Description = "This email have already been registered.", Code = "RegisteredEmail" });
            }
        }
    }
}
