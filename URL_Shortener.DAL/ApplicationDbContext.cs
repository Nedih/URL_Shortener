namespace URL_Shortener.DAL
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using URL_Shortener.DAL.Entities;

    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Url> Urls { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
