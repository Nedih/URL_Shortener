using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models.Identity;
using URL_Shortener.Server.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace URL_Shortener.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserAuthService> _mockAuthService;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IUserAuthService>();
            _authController = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task RegisterUser_ReturnsCreatedResult_WhenRegistrationIsSuccessful()
        {
            var registerModel = new RegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };
            var identityResult = IdentityResult.Success;
            _mockAuthService.Setup(service => service.RegisterUserAsync(registerModel))
                            .ReturnsAsync(identityResult);

            var result = await _authController.RegisterUser(registerModel);

            var actionResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, actionResult.StatusCode);
        }

        [Fact]
        public async Task RegisterUser_ReturnsBadRequest_WhenRegistrationFails()
        {
            var registerModel = new RegisterViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Error" });
            _mockAuthService.Setup(service => service.RegisterUserAsync(registerModel))
                            .ReturnsAsync(identityResult);

            var result = await _authController.RegisterUser(registerModel);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(identityResult, actionResult.Value);
        }

        [Fact]
        public async Task Authenticate_ReturnsOkResult_WithTokens_WhenLoginIsSuccessful()
        {
            var loginModel = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };
            var loginResult = new AuthResponse
            {
                Succeeded = true,
                Tokens = new TokenModel
                {
                    AccessToken = "access_token",
                    RefreshToken = "refresh_token"
                },
                Roles = new List<string> { "User" }
            };
            _mockAuthService.Setup(service => service.ValidateUserAsync(loginModel))
                            .ReturnsAsync(loginResult);

            var result = await _authController.Authenticate(loginModel);

            var actionResult = Assert.IsType<OkObjectResult>(result);

            var expectedResult = JsonConvert.SerializeObject(new
            {
                loginResult.Tokens?.AccessToken,
                loginResult.Tokens?.RefreshToken,
                loginResult.Roles
            });
            Assert.Equal(expectedResult, JsonConvert.SerializeObject(actionResult.Value));
        }

        [Fact]
        public async Task Authenticate_ReturnsUnauthorized_WhenLoginFails()
        {
            var loginModel = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };
            var loginResult = new AuthResponse
            {
                Succeeded = false,
                Error = new AuthError("WrongEmail", "There is no account with this email")
            };
            _mockAuthService.Setup(service => service.ValidateUserAsync(loginModel))
                            .ReturnsAsync(loginResult);

            var result = await _authController.Authenticate(loginModel);

            var actionResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(loginResult.Error, actionResult.Value);
        }
    }
}