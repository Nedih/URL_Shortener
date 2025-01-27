using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace URL_Shortener.DAL.Entities
{
    public class UserAccount : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public List<Url> Urls { get; set; } = new List<Url>();
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
