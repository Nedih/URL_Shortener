using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener.BLL.Models.ViewModels;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models;
using FluentResults;

namespace URL_Shortener.API.Controllers
{
    public class UrlController : Controller
    {
        private readonly IUrlService _service;

        public UrlController(IUrlService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<UrlDTO> GetUrls()
        {
            return _service.GetUrls();
        }
        [Authorize]
        [HttpPost]
        public async Task<Result> CreateUrl([FromBody] UrlDTO url)
        {
            return await _service.CreateAsync(url);
        }

        [Authorize]
        [HttpDelete]
        public Result DeleteAsync(long id)
        {
            return _service.Delete(id).IsSuccess ? Result.Ok() : Result.Fail(_service.Delete(id).Errors);
        }

        /*[Authorize]
        [HttpGet("{id}")]
        public async Task<UrlDTO?> GetPost(Guid id)
        {
            return await _service.GetUrl(id);
        }    */
    }
}
