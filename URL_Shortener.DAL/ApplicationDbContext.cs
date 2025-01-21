namespace URL_Shortener.DAL
{
    using System;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using URL_Shortener.DAL.Entities;

    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Url> Urls { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Url>()
                .HasKey(u => u.UrlId);

            modelBuilder.Entity<Url>()
                .Property(u => u.UrlId)
                .ValueGeneratedNever();

            var users = SeedUsers(modelBuilder, 3);
            var roles = SeedRoles(modelBuilder);
            SeedUserRoles(modelBuilder, users, roles);
        }

        private List<UserAccount> SeedUsers(ModelBuilder builder, int count)
        {
            List<UserAccount> seedUsers = new List<UserAccount>();
            
            for(int i = 1; i <= count; i++) 
            {
                string admin = $"admin{i}@admin.com";
                seedUsers.Add(new UserAccount
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = $"Admin{i}",
                    UserName = admin,
                    NormalizedUserName = admin.ToUpper(),
                    Email = admin,
                    NormalizedEmail = admin.ToUpper()
                });

                string user = $"user{i}@user.com";
                seedUsers.Add(new UserAccount
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = $"User{i}",
                    UserName = user,
                    NormalizedUserName = user.ToUpper(),
                    Email = user,
                    NormalizedEmail = user.ToUpper()
                });
            }

            var ph = new PasswordHasher<UserAccount>();

            foreach (var user in seedUsers)
                user.PasswordHash = ph.HashPassword(user, "Qwerty_123");

            builder.Entity<UserAccount>().HasData(seedUsers);
            return seedUsers;
        }

        private List<IdentityRole> SeedRoles(ModelBuilder builder)
        {
            List<IdentityRole> seedRoles = new List<IdentityRole>
            { 
                new IdentityRole() { Id = "47e17bad-e591-4084-b31c-40c1e4859bd7", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Id = "1c1ebabd-6745-4fc2-808d-48df8107736c", Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
            };
            builder.Entity<IdentityRole>().HasData(seedRoles);
            return seedRoles;
        }

        private void SeedUserRoles(ModelBuilder builder, List<UserAccount> users, List<IdentityRole> roles)
        {
            List<IdentityUserRole<string>> seedUserRoles = new List<IdentityUserRole<string>>();
            foreach (var user in users)
            {
                IdentityUserRole<string> userRole = new IdentityUserRole<string>()
                {
                    UserId = user.Id
                };
                userRole.RoleId = user.UserName.Contains("Admin")
                    ? roles.FirstOrDefault(x => x.Name == "Admin").Id
                    : roles.FirstOrDefault(x => x.Name == "User").Id;
                seedUserRoles.Add(userRole);
            }
            builder.Entity<IdentityUserRole<string>>().HasData(seedUserRoles);
        }
    }
}
