using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentResults;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models;
using URL_Shortener.Server.Controllers;
using Newtonsoft.Json;

namespace URL_Shortener.Tests.Controllers
{
    public class UrlControllerTests
    {
        private readonly Mock<IUrlService> _mockUrlService;
        private readonly UrlController _urlController;

        public UrlControllerTests()
        {
            _mockUrlService = new Mock<IUrlService>();
            _urlController = new UrlController(_mockUrlService.Object);
        }

        [Fact]
        public async Task CreateUrl_ReturnsOkResult_WhenCreationIsSuccessful()
        {
            var urlDto = new UrlDTO { UrlText = "https://example.com" };
            var expectedShortUrl = "https://short.ly/abc123";
            _mockUrlService.Setup(service => service.CreateAsync(urlDto))
                           .ReturnsAsync(Result.Ok(expectedShortUrl));

            var result = await _urlController.CreateUrl(urlDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(JsonConvert.SerializeObject(new { shortenUrl = "https://short.ly/abc123" }), JsonConvert.SerializeObject(okResult.Value));
        }

        [Fact]
        public async Task CreateUrl_ReturnsBadRequest_WhenCreationFails()
        {
            var urlDto = new UrlDTO { UrlText = "https://example.com" };
            var errorMessage = "Invalid URL";
            _mockUrlService.Setup(service => service.CreateAsync(urlDto))
                           .ReturnsAsync(Result.Fail(errorMessage));

            var result = await _urlController.CreateUrl(urlDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }
    }
}
