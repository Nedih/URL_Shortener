using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models;
using URL_Shortener.Server.Controllers;

namespace URL_Shortener.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(_mockUserService.Object);
        }

        [Fact]
        public void Get_ReturnsAllUsers_WhenUserIsAdmin()
        {
            var users = new List<UserDTO>
        {
            new UserDTO { Id = "1", Email = "admin@example.com" },
            new UserDTO { Id = "2", Email = "user@example.com" }
        };
            _mockUserService.Setup(service => service.GetUsers()).Returns(users);

            var result = _userController.Get();

            Assert.Equal(users, result);
        }

        [Fact]
        public void GetList_ReturnsUserProfiles_WhenUserIsAuthenticated()
        {
            var profiles = new List<ProfileDTO>
        {
            new ProfileDTO { Login = "user1@example.com" },
            new ProfileDTO { Login = "user2@example.com" }
        };
            _mockUserService.Setup(service => service.GetUsersList()).Returns(profiles);

            var result = _userController.GetList();

            Assert.Equal(profiles, result);
        }
    }
}
