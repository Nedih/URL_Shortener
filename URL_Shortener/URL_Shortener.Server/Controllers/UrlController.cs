﻿using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener.BLL.Models.ViewModels;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models;
using FluentResults;

namespace URL_Shortener.Server.Controllers
{
    [Route("api/url")]
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
        public Result DeleteAsync(string url)
        {
            return _service.Delete(url).IsSuccess ? Result.Ok() : Result.Fail(_service.Delete(url).Errors);
        }

        /*[Authorize]
        [HttpGet("{id}")]
        public async Task<UrlDTO?> GetPost(Guid id)
        {
            return await _service.GetUrl(id);
        }    */
    }
}
