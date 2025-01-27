using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Services;
using URL_Shortener.DAL.Entities;
using URL_Shortener.DAL.Interfaces;
using URL_Shortener.DAL.Repositories;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace URL_Shortener.Server.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"];
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["validIssuer"],
                    ValidAudiences = new List<string>
                    {
                        jwtConfig["validAudienceBack"],
                        jwtConfig["validAudienceFront"]
                    },
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pocom API",
                    Version = "v1",
                    Description = "Pocom educational project API Services.",
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
            });
        }
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IUserAuthService, AuthService>();
            services.AddTransient<IRepository<Url>, Repository<Url>>();
            services.AddTransient<IRepository<UserAccount>, Repository<UserAccount>>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUrlService, UrlService>();
        }
    }
}
