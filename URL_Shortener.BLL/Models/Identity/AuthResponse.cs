using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;

namespace URL_Shortener.BLL.Models.Identity
{
    public class AuthResponse
    {
        public bool Succeeded { get; set; }
        public TokenModel? Tokens { get; set; }
        public ICollection<string>? Roles { get; set; }
        public AuthError? Error { get; set; }

        public static AuthResponse Success(TokenModel tokens, ICollection<string> roles)
        {
            return new AuthResponse
            {
                Succeeded = true,
                Tokens = tokens,
                Roles = roles,
                Error = null
            };
        }
        public static AuthResponse Success(dynamic data)
        {
            return new AuthResponse
            {
                Succeeded = true,
                Tokens = data.Tokens,
                Roles = data.Roles,
                Error = null
            };
        }
        public static AuthResponse Failure(AuthError error)
        {
            return new AuthResponse
            {
                Succeeded = false,
                Tokens = null,
                Roles = null,
                Error = error
            };
        }
        public static AuthResponse Failure(dynamic data)
        {
            return new AuthResponse
            {
                Succeeded = false,
                Tokens = null,
                Roles = null,
                Error = data.Error
            };
        }
    }
}
