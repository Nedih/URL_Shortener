using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models.Identity;
using URL_Shortener.BLL.Services;
using URL_Shortener.DAL.Entities;
using Xunit;

namespace URL_Shortener.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<UserAccount>> _userManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userManagerMock = MockUserManager<UserAccount>();

            var jwtConfigSectionMock = new Mock<IConfigurationSection>();
            jwtConfigSectionMock.Setup(x => x["validIssuer"]).Returns("https://localhost:5142/");
            jwtConfigSectionMock.Setup(x => x["validAudience"]).Returns("https://localhost:7498/");
            jwtConfigSectionMock.Setup(x => x["validAudienceFront"]).Returns("https://localhost:7498/");
            jwtConfigSectionMock.Setup(x => x["validAudienceBack"]).Returns("https://localhost:7214/");
            jwtConfigSectionMock.Setup(x => x["secret"]).Returns("WXJSAMFKLSTEHSYOVWXJSAMFKLSTEHSYOV");
            jwtConfigSectionMock.Setup(x => x["expiresIn"]).Returns("10");
            jwtConfigSectionMock.Setup(x => x["RefreshTokenValidityInDays"]).Returns("7");

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x.GetSection("jwtConfig")).Returns(jwtConfigSectionMock.Object);

            _authService = new AuthService(_userManagerMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var registerModel = new RegisterViewModel
            {
                Email = "test@example.com",
                Password = "Test@1234",
                Name = "Test User"
            };

            _userManagerMock
                .Setup(um => um.CreateAsync(It.IsAny<UserAccount>(), registerModel.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(um => um.AddToRoleAsync(It.IsAny<UserAccount>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.RegisterUserAsync(registerModel);

            // Assert
            Assert.True(result.Succeeded);
            _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<UserAccount>(), registerModel.Password), Times.Once);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<UserAccount>(), "User"), Times.Once);
        }

        [Fact]
        public async Task ValidateUserAsync_InvalidEmail_ReturnsFailure()
        {
            // Arrange
            var loginModel = new LoginViewModel
            {
                Email = "invalid@example.com",
                Password = "Test@1234"
            };

            //_userManagerMock
                //.Setup(um => um.FindByNameAsync(loginModel.Email))
                //.ReturnsAsync((UserAccount?)null);

            // Act
            var result = await _authService.ValidateUserAsync(loginModel);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("WrongEmail", result?.Error?.Code);
        }

        [Fact]
        public async Task ValidateUserAsync_ValidEmailAndPassword_ReturnsSuccess()
        {
            // Arrange
            var loginModel = new LoginViewModel
            {
                Email = "admin2@admin.com",
                Password = "Qwerty_123"
            };

            var user = new UserAccount { Email = "admin2@admin.com" };

            _userManagerMock
                .Setup(um => um.FindByNameAsync(loginModel.Email))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(um => um.CheckPasswordAsync(user, loginModel.Password))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(um => um.IsLockedOutAsync(user))
                .ReturnsAsync(false);

            _userManagerMock
                .Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            _configurationMock
                .Setup(cfg => cfg.GetSection("JwtConfig").GetSection("Secret").Value)
                .Returns("WXJSAMFKLSTEHSYOVWXJSAMFKLSTEHSYOV");

            // Act
            var result = await _authService.ValidateUserAsync(loginModel);

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Tokens);
            Assert.NotNull(result.Roles);
            Assert.Contains("User", result.Roles);
        }

        [Fact]
        public async Task RefreshTokensAsync_InvalidAccessToken_ReturnsFailure()
        {
            // Arrange
            var tokenModel = new TokenModel
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbjJAYWRtaW4uY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI0MzNhYWY3MC0zYzYwLTQ4NTMtYmYwYy1kODQ4NTI3NGFjMjQiLCJqdGkiOiI0Zjc3ZWJjMC1jOTI3LTQzY2QtYjZmYS1kZTAwNzBhZGIwMGEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiVXNlciIsIkFkbWluIl0sImV4cCI6MTczNzg5OTUxMSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTE0Mi8iLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3NDk4LyJ9.JbS1gzlHp6sm7MXBzvPQStCSXv3wDY8Tt7jmaUu-R9E",
                RefreshToken = "AWF14sknxa/3sby27H9KvzY0faDRrOGJG/UoTvUr/b0ZQ7VBg/WkE4mltwg+VeOtrfnapPJicN4zeKLACGdypg=="
            };

            _configurationMock
                .Setup(cfg => cfg.GetSection("JwtConfig").GetSection("Secret").Value)
                .Returns("WXJSAMFKLSTEHSYOVWXJSAMFKLSTEHSYOV");

            // Act
            var result = await _authService.RefreshTokensAsync(tokenModel);

            // Assert
            Assert.NotNull(result.Exception);
            Assert.Equal("Invalid access token", result.Exception);
        }

        [Fact]
        public async Task GetUserClaimsAsync_ValidUser_ReturnsRoles()
        {
            // Arrange
            var user = new UserAccount { Email = "test@example.com" };
            _userManagerMock
                .Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin", "User" });

            // Act
            var claims = await _authService.GetUserClaimsAsync(user);

            // Assert
            Assert.NotEmpty(claims);
            Assert.Contains("Admin", claims);
            Assert.Contains("User", claims);
        }

        private Mock<UserManager<T>> MockUserManager<T>() where T : class
        {
            var store = new Mock<IUserStore<T>>();
            return new Mock<UserManager<T>>(
                store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
