using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URL_Shortener.DAL.Entities;

namespace URL_Shortener.BLL.Models
{
    public class UrlDTO
    {
        public string UrlText { get; set; }
        public string ShortenUrl { get; set; }
        public string UrlCreationDate { get; set; }
        public string? UrlDescription { get; set; }

        public string UserId { get; set; }
    }
}
