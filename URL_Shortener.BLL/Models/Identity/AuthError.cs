using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URL_Shortener.BLL.Models.Identity
{
    public class AuthError
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }

        public AuthError(string code, string description) 
        { 
            Code = code;
            Description = description;
        }

        public AuthError(dynamic data)
        {
            Code = data.Code;
            Description = data.Description;
        }
    }
}
