using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace URL_Shortener.DAL.Entities
{
    public class UserAccount : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string DateOfBirth { get; set; }

        public List<Url> Urls { get; set; } 
    }
}
