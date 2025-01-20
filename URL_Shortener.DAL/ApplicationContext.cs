namespace URL_Shortener.DAL
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using URL_Shortener.DAL.Entities;

    public class ApplicationContext : DbContext
    {
        public DbSet<Entity> Entities { get; set; }

        public string DbPath { get; }

        public ApplicationContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "shortener.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
