using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using URL_Shortener.BLL.Interfaces;
using URL_Shortener.BLL.Services;
using URL_Shortener.DAL.Entities;
using URL_Shortener.DAL.Interfaces;
using URL_Shortener.DAL.Repositories;
using System.Text;

namespace URL_Shortener.Server.Extensions
{
    public static class ServiceCollectionExtension
    {
        
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IUserAuthService, UserAuthService>();
            services.AddTransient<IRepository<Url>, Repository<Url>>();
            services.AddTransient<IRepository<UserAccount>, Repository<UserAccount>>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUrlService, UrlService>();
        }
    }
}
