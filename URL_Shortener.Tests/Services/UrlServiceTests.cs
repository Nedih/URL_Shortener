using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using URL_Shortener.BLL.Models;
using URL_Shortener.BLL.Services;
using URL_Shortener.DAL.Entities;
using URL_Shortener.DAL.Interfaces;

namespace URL_Shortener.Tests.Services
{
    public class UrlServiceTests
    {
        private readonly Mock<IRepository<Url>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<UserManager<UserAccount>> _userManagerMock;
        private readonly UrlService _urlService;

        public UrlServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Url>>();
            _mapperMock = new Mock<IMapper>();

            var store = new Mock<IUserStore<UserAccount>>();
            _userManagerMock = new Mock<UserManager<UserAccount>>(
                store.Object, null, null, null, null, null, null, null, null);

            _urlService = new UrlService(
                _repositoryMock.Object,
                _mapperMock.Object,
                _userManagerMock.Object);
        }

        [Fact]
        public async Task CreateAsync_UrlAlreadyExists_ReturnsFailure()
        {
            var urlDto = new UrlDTO { UrlText = "https://existing-url.com" };
            _repositoryMock
                .Setup(repo => repo.FirstOrDefault(It.IsAny<Func<Url, bool>>()))
                .Returns(new Url());

            var result = await _urlService.CreateAsync(urlDto);

            Assert.True(result.IsFailed);
            Assert.Equal("This URL is already in our DataBase", result.Errors[0].Message);
        }

        [Fact]
        public async Task CreateAsync_UserNotFound_ReturnsFailure()
        {
            var urlDto = new UrlDTO { UrlText = "https://new-url.com", UserEmail = "user3@user.com" };
            _repositoryMock
                .Setup(repo => repo.FirstOrDefault(It.IsAny<Func<Url, bool>>()))
                .Returns((Url)null);
            _userManagerMock
                .Setup(um => um.FindByEmailAsync(urlDto.UserEmail))
                .ReturnsAsync((UserAccount)null);

            var result = await _urlService.CreateAsync(urlDto);

            Assert.True(result.IsFailed);
            Assert.Equal("User with this email doesn't exist", result.Errors[0].Message);
        }

        [Fact]
        public async Task CreateAsync_ValidUrl_ReturnsShortUrl()
        {
            var urlDto = new UrlDTO { UrlText = "https://new-url.com", UserEmail = "user3@user.com" };
            var user = new UserAccount { Email = urlDto.UserEmail };
            var urlEntity = new Url { UrlId = 1, UrlText = urlDto.UrlText };

            _repositoryMock
                .Setup(repo => repo.FirstOrDefault(It.IsAny<Func<Url, bool>>()))
                .Returns((Url)null);
            _userManagerMock
                .Setup(um => um.FindByEmailAsync(urlDto.UserEmail))
                .ReturnsAsync(user);
            _mapperMock
                .Setup(mapper => mapper.Map<Url>(urlDto))
                .Returns(urlEntity);
            _repositoryMock
                .Setup(repo => repo.Add(It.IsAny<Url>()))
                .Callback<Url>(url => url.UrlId = 1);

            var result = await _urlService.CreateAsync(urlDto);

            Assert.True(result.IsSuccess);
            Assert.Equal("1", result.Value); 
        }
    }
}
