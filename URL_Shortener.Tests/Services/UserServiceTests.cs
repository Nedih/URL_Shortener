using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models;
using URL_Shortener.BLL.Models.Identity;
using URL_Shortener.BLL.Services;
using URL_Shortener.DAL.Entities;
using URL_Shortener.DAL.Interfaces;
using Xunit;

namespace URL_Shortener.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IRepository<UserAccount>> _userRepoMock;
        private readonly Mock<UserManager<UserAccount>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IRepository<UserAccount>>();
            _userManagerMock = MockUserManager();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepoMock.Object, _userManagerMock.Object, _mapperMock.Object);
        }

        private Mock<UserManager<UserAccount>> MockUserManager()
        {
            var store = new Mock<IUserStore<UserAccount>>();
            return new Mock<UserManager<UserAccount>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public void GetUsers_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<UserAccount>
            {
                new UserAccount { Id = "1", UserName = "user1@example.com", Email = "user1@example.com" },
                new UserAccount { Id = "2", UserName = "user2@example.com", Email = "user2@example.com" }
            };
            _userManagerMock.Setup(um => um.Users).Returns(users.AsQueryable());
            var userDTOs = new List<UserDTO>
            {
                new UserDTO { Id = "1", Email = "user1@example.com" },
                new UserDTO { Id = "2", Email = "user2@example.com" }
            };
            _mapperMock.Setup(m => m.Map<List<UserDTO>>(It.IsAny<List<UserAccount>>())).Returns(userDTOs);

            // Act
            var result = _userService.GetUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetUser_ReturnsUserById()
        {
            // Arrange
            var user = new UserAccount { Id = "1", UserName = "user1@example.com", Email = "user1@example.com" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            var userDTO = new UserDTO { Id = "1", Email = "user1@example.com" };
            _mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<UserAccount>())).Returns(userDTO);

            // Act
            var result = await _userService.GetUser("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal("user1@example.com", result.Email);
        }

        [Fact]
        public async Task UpdateUser_UpdatesExistingUser()
        {
            // Arrange
            var user = new UserAccount { Id = "1", UserName = "user1@example.com", Email = "user1@example.com" };
            var userDTO = new UserDTO { Id = "1", Email = "newemail@example.com" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map(It.IsAny<UserDTO>(), It.IsAny<UserAccount>())).Callback<UserDTO, UserAccount>((src, dest) =>
            {
                dest.Email = src.Email;
            });
            _userManagerMock.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
            _userRepoMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUser("1", userDTO);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("newemail@example.com", user.Email);
        }

        [Fact]
        public async Task CreateUser_CreatesNewUser()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel { Email = "newuser@example.com", Password = "Password123!" };
            var user = new UserAccount { Id = "1", UserName = "newuser@example.com", Email = "newuser@example.com" };
            _userManagerMock.Setup(um => um.FindByEmailAsync("newuser@example.com")).ReturnsAsync((UserAccount)null);
            _mapperMock.Setup(m => m.Map<UserAccount>(It.IsAny<RegisterViewModel>())).Returns(user);
            _userManagerMock.Setup(um => um.CreateAsync(user, registerViewModel.Password)).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.AddToRoleAsync(user, "User")).ReturnsAsync(IdentityResult.Success);
            _userRepoMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.CreateUser(registerViewModel);

            // Assert
            Assert.True(result.Succeeded);
            _userManagerMock.Verify(um => um.AddToRoleAsync(user, "User"), Times.Once);
            _userRepoMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

    }
}
