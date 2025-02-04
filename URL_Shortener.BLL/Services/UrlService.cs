﻿using System.Text;
using System;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models;
using URL_Shortener.DAL.Entities;
using URL_Shortener.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace URL_Shortener.BLL.Services
{
    public class UrlService : IUrlService
    {
        private readonly IRepository<Url> _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<UserAccount> _userManager;
        private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";
        private readonly IDictionary<char, int> AlphabetIndex;
        private readonly int Base = Alphabet.Length;

        public UrlService(IRepository<Url> repository, IMapper mapper, UserManager<UserAccount> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            AlphabetIndex = Alphabet
                .Select((c, i) => new { Index = i, Char = c })
                .ToDictionary(c => c.Char, c => c.Index);
        }
        public async Task<Result<string>> CreateAsync(UrlDTO urlModel)
        {
            try
            {
                if (_repository.FirstOrDefault(x => x.UrlText == urlModel.UrlText) != null)
                    return Result.Fail("This URL is already in our DataBase");
                
                var author = await _userManager.FindByEmailAsync(urlModel.UserEmail);

                if (author == null) return Result.Fail("User with this email doesn't exist");

                var url = _mapper.Map<Url>(urlModel);

                var shortURL = Encode(url.UrlId);
                url.ShortenUrl = Decode(shortURL) == url.UrlId? shortURL: throw new Exception("Shorten URL was corrupted");

                url.UrlCreationDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:FFF");

                url.UserAccount = author;

                _repository.Add(url);
                
                return Result.Ok(shortURL);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex?.InnerException?.Message);
            }
        }

        private string Encode(long num) 
        {
            if (num == 0) return Alphabet[0].ToString();

            StringBuilder result = new StringBuilder();
            while (num > 0)
            {
                int remainder = (int)(num % Base);
                result.Insert(0, Alphabet[remainder]);
                num /= Base;
            }

            return result.ToString();
        }

        private long Decode(string encoded)
        {
            long num = 0;
            foreach (char c in encoded)
            {
                int index = Alphabet.IndexOf(c);
                if (index == -1) throw new ArgumentException("Invalid character in encoded string.");
                num = num * Base + index;
            }

            return num;
        }

        public Result Delete(string url)
        {
            try
            {
                var entity = _repository.FirstOrDefault(x => x.ShortenUrl == url);
                if (entity == null)
                    return Result.Fail(new Error("There is no such entity"));

                _repository.Remove(entity);
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error(ex?.InnerException?.Message));
            }
            return Result.Ok();
        }

        public Result Update(UrlDTO url)
        {
            try
            {
                _repository.Update(_mapper.Map<Url>(url));
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error(ex?.InnerException?.Message));
            }
            return Result.Ok();
        }

        public List<UrlDTO> GetUrls()
        {
            return _mapper.Map<List<UrlDTO>>(_repository.GetAll().Include(x => x.UserAccount));
        }

        public UrlDTO? GetUrl(string url)
        {
            return _mapper.Map<UrlDTO>(_repository.Include(x => x.UserAccount).FirstOrDefault(x => x.ShortenUrl == url));
        }
    }
}
