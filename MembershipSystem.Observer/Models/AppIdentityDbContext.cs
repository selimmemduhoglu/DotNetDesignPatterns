﻿using Microsoft.EntityFrameworkCore.Design;

namespace MembershipSystem.Observer.Models
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        public DbSet<Discount> Discounts { get; set; }
        public class AppIdentityDbContextFactory : IDesignTimeDbContextFactory<AppIdentityDbContext>
        {
            public AppIdentityDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>();
                optionsBuilder.UseSqlServer("Server=localhost,1433;Database=DesignPattern;User=sa;Password=Password123");

                return new AppIdentityDbContext(optionsBuilder.Options);
            }
        }
    }
}