using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models.Identity;
using URL_Shortener.DAL.Entities;

namespace URL_Shortener.BLL.Services
{
    public class AuthService : IUserAuthService
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly IConfiguration _configuration;
        private UserAccount? _user;

        public AuthService(UserManager<UserAccount> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<List<string>> GetUserClaimsAsync(UserAccount user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return [.. roles];
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {
            UserAccount user = new UserAccount
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Errors.Count() == 0)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            return result;
        }

        public async Task<AuthResponse> ValidateUserAsync(LoginViewModel model)
        {
            _user = await _userManager.FindByNameAsync(model.Email);
            if (_user == null)
                return AuthResponse.Failure(new AuthError("WrongEmail", "There is no account with this email")); 
            if (!await _userManager.CheckPasswordAsync(_user, model.Password))
                return AuthResponse.Failure(new AuthError("WrongPassword", "You entered wrong password")); 
            if (await _userManager.IsLockedOutAsync(_user))
                return AuthResponse.Failure(new AuthError("UserLocked", "Your account is locked"));
            var tokens = await CreateTokensAsync(_user);
            var roles = await GetUserClaimsAsync(_user);
            return AuthResponse.Success(new { Tokens = tokens, Roles = roles });
        }

        public async Task<TokenModel> CreateTokensAsync(UserAccount user)
        {
            return new TokenModel
            {
                AccessToken = await CreateTokenAsync(),
                RefreshToken = await GenerateRefreshToken(user)
            };
        }

        public async Task<TokenModel> RefreshTokensAsync(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return new TokenModel { Exception = "Invalid client request" };
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            ClaimsPrincipal? principal;

            principal = GetPrincipalFromExpiredToken(accessToken);

            if (principal == null)
            {
                return new TokenModel { Exception = "Invalid access token" };
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) 
            {
                return new TokenModel { Exception = "Invalid access token" };
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new TokenModel { Exception = "Invalid access token" };
            }

            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new TokenModel { Exception = "Invalid refresh token" };
            }
            string? token;
            if (principal.FindFirstValue(ClaimTypes.Email) == user.Email)
                token = CreateTokenAsync(principal.Claims.ToList());
            else token = await CreateTokenAsync();
            return new TokenModel
            {
                AccessToken = token,
                RefreshToken = await GenerateRefreshToken(user)
            };
        }

        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private string CreateTokenAsync(List<Claim> claims)
        {
            var signingCredentials = GetSigningCredentials();
            //var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("jwtConfig");
            var jwtSecret = jwtConfig["secret"] 
                ?? throw new ArgumentNullException(
                    "secret",
                    "JWT secret is missing in the configuration."
                );
            var key = Encoding.UTF8.GetBytes(jwtSecret);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _user!.Email!),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudienceFront"],//"http://localhost:3000/",//jwtSettings["validAudienceFront"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresIn"])),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        private async Task<string> GenerateRefreshToken(UserAccount user)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var refreshToken = Convert.ToBase64String(randomNumber);

            _ = int.TryParse(_configuration.GetSection("JwtConfig")["RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            await _userManager.UpdateAsync(user);

            return refreshToken;
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var jwtConfig = _configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"] 
                ?? throw new ArgumentNullException(
                    "secret", 
                    "JWT secret is missing in the configuration."
                );
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["validIssuer"] 
                    ?? throw new ArgumentNullException(
                        "validIssuer", 
                        "JWT valid issuer is missing in the configuration."
                    ),
                ValidAudiences = new List<string>
                {
                    jwtConfig["validAudienceBack"] 
                    ?? throw new ArgumentNullException(
                        "validAudienceBack",
                        "JWT valid audience(s) are missing in the configuration."
                    ),
                    jwtConfig["validAudienceFront"] 
                    ?? throw new ArgumentNullException(
                        "validAudienceFront",
                        "JWT valid audience(s) are missing in the configuration."
                    )
                }.AsEnumerable(),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
