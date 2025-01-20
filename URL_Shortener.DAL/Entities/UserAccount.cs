using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using static System.Collections.Specialized.BitVector32;

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


    }
}
