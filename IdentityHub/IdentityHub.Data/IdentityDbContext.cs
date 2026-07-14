using Microsoft.EntityFrameworkCore;
using IdentityHub.Core.Entities;

namespace IdentityHub.Data
{
    public class IdentityDbContext : DbContext
    {
       
        public IdentityDbContext(DbContextOptions options) : base(options) { }

        public DbSet<RegisteredApplication> RegisteredApplications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<UserAppPermission> UserAppPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegisteredApplication>().HasIndex(a => a.ClientId).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
          ;
        }
    }
}
