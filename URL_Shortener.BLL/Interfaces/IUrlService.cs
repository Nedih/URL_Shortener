﻿using FluentResults;
using URL_Shortener.BLL.Models;

namespace URL_Shortener.BLL.Interfaces
{
    public interface IUrlService
    {
        public Task<Result> CreateAsync(UrlDTO url);
        public Result Update(UrlDTO url);
        public Result Delete(long id);
        public List<UrlDTO> GetUrls();
    }
}
