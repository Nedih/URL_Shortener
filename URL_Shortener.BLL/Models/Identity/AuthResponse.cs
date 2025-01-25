using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URL_Shortener.BLL.Models.Identity
{
    public class AuthResponse
    {
        public bool Succeeded { get; set; }
        public TokenModel? Tokens { get; set; }
        public ICollection<string>? Roles { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public static AuthResponse Success(TokenModel tokens, ICollection<string> roles)
        {
            return new AuthResponse
            {
                Succeeded = true,
                Tokens = tokens,
                Roles = roles,
                Errors = null
            };
        }
        public static AuthResponse Success(dynamic data)
        {
            return new AuthResponse
            {
                Succeeded = true,
                Tokens = data.Tokens,
                Roles = data.Roles,
                Errors = null
            };
        }
        public static AuthResponse Failure(IEnumerable<string> errors)
        {
            return new AuthResponse
            {
                Succeeded = false,
                Tokens = null,
                Roles = null,
                Errors = errors
            };
        }
        public static AuthResponse Failure(dynamic data)
        {
            return new AuthResponse
            {
                Succeeded = false,
                Tokens = null,
                Roles = null,
                Errors = data.Errors
            };
        }
    }
}
