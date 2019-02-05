using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Entities
{
    public class HalcyonDbContext : DbContext
    {
        public HalcyonDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(a => a.EmailAddress)
                .IsUnique();

            modelBuilder.Entity<UserRefreshToken>()
                .HasIndex(a => a.Token)
                .IsUnique();

            modelBuilder.Entity<UserLogin>()
                .HasIndex(a => new { a.Provider, a.ExternalId })
                .IsUnique();
        }

        public DbSet<User> Users { get; set; }
    }
}