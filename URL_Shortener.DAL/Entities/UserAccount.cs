using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace URL_Shortener.DAL.Entities
{
    public class UserAccount : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public List<Url> Urls { get; set; } 
    }
}
