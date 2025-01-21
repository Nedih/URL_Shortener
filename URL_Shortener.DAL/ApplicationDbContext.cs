namespace URL_Shortener.DAL
{
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
            var users = SeedUsers(modelBuilder);
            var roles = SeedRoles(modelBuilder);
            SeedUserRoles(modelBuilder, users, roles);
        }

        private List<UserAccount> SeedUsers(ModelBuilder builder)
        {
            List<UserAccount> seedUsers = new List<UserAccount>
            {
                new UserAccount{
                    //Id = "b74ddd14-6wp0-4gg0-95c2-db12554843e1",
                    Id = Guid.NewGuid().ToString(),
                    Name = "User1",
                    UserName = "User1",
                    Email = "user1@user.com",
                },
                new UserAccount{
                    Id = Guid.NewGuid().ToString(),
                    Name = "User2",
                    UserName = "User2",
                    Email = "user2@user.com"
                },
                new UserAccount{
                    Id = Guid.NewGuid().ToString(),
                    Name = "User3",
                    UserName = "User3",
                    Email = "user3@user.com"
                },
                new UserAccount{
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin1",
                    UserName = "Admin1",
                    Email = "admin1@user.com"
                },
                new UserAccount{
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin2",
                    UserName = "Admin2",
                    Email = "admin2@user.com"
                },
                new UserAccount{
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin3",
                    UserName = "Admin3",
                    Email = "admin3@user.com"
                }
            };

            var ph = new PasswordHasher<UserAccount>();

            foreach (var user in seedUsers)
                user.PasswordHash = ph.HashPassword(user, "qwerty123");

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
