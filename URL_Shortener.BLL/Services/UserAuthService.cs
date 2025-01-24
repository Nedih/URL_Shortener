using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Models.Identity;
using URL_Shortener.DAL.Entities;

namespace URL_Shortener.BLL.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly UserManager<UserAccount> _userManager;
        //private readonly RoleManager<UserAccount> _roleManager;
        private readonly IConfiguration _configuration;
        private UserAccount? _user;

        public UserAuthService(UserManager<UserAccount> userManager, /*RoleManager<UserAccount> roleManager,*/ IConfiguration configuration)
        {
            _userManager = userManager;
            //_roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<List<string>> GetUserClaimsAsync(LoginViewModel userModel)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
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

        public async Task<IdentityResult> ValidateUserAsync(LoginViewModel model)
        {
            _user = await _userManager.FindByNameAsync(model.Email);
            if (_user == null)
                return IdentityResult.Failed(new IdentityError { Description = "There is no account with such email", Code = "WrongEmail" });
            if (!await _userManager.CheckPasswordAsync(_user, model.Password))
                return IdentityResult.Failed(new IdentityError { Description = "You entered wrong password", Code = "WrongPassword" });
            if (await _userManager.IsLockedOutAsync(_user))
                return IdentityResult.Failed(new IdentityError { Description = "Your account is locked", Code = "UserLocked" });

            return IdentityResult.Success;
        }

        public async Task<TokenModel> CreateTokensAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
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
                return new TokenModel { Exception = "Invalid access token or refresh token" };
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new TokenModel { Exception = "Invalid access token or refresh token" };
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
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _user.Email),
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
            var secretKey = jwtConfig["secret"];
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["validIssuer"],
                
                ValidAudiences = new List<string>
                {
                    jwtConfig["validAudienceBack"],
                    jwtConfig["validAudienceFront"]
                },
                //ValidAudience = "http://localhost:3000/",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (securityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
