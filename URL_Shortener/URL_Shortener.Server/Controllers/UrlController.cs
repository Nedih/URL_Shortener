using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener.BLL.Models.ViewModels;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;

namespace URL_Shortener.Server.Controllers
{
    [Route("api/url")]
    public class UrlController(IUrlService service) : Controller
    {
        private readonly IUrlService _service = service;

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<UrlDTO> GetUrls()
        {
            return _service.GetUrls();
        }
        [Authorize(Policy = "UserPolicy")]
        [HttpPost("create")]
        //[Authorize]
        public async Task<IActionResult> CreateUrl([FromBody] UrlDTO url)
        {
            var result = await _service.CreateAsync(url);
            if (result.IsSuccess)
            {
                return Ok(new{ shortenUrl = result.Value });
            }

            return BadRequest(string.Join(", ", result.Errors.Select(error => error.Message)));
        }

        [Authorize(Policy = "UserPolicy")]
        //[AllowAnonymous]
        [HttpDelete]
        public Result Delete(string shorten)
        {
            var res = _service.Delete(shorten);
            return res.IsSuccess ? Result.Ok() : Result.Fail(res.Errors);
        }

        //[Authorize(Roles = "User, Admin")]
        [HttpGet("{shorten}")]
        public UrlDTO? GetPost(string shorten)
        {
            return _service.GetUrl(shorten);
        }    
    }
}
