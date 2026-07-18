using Microsoft.EntityFrameworkCore;
using SportsFacility.Domain.Models;
using System.Reflection.Emit;

namespace SportsFacility.Domain.Infra
{
    // Data/SportsFacilityDbContext.cs
    public class SportsFacilityDbContext : DbContext
    {
        public SportsFacilityDbContext(DbContextOptions<SportsFacilityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<FacilityEquipment> Equipment { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Facility relationships
            modelBuilder.Entity<Facility>()
                .HasMany(f => f.Bookings)
                .WithOne(b => b.Facility)
                .HasForeignKey(b => b.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Facility>()
                .HasMany(f => f.Equipment)
                .WithOne(e => e.Facility)
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Member relationships
            modelBuilder.Entity<Member>()
                .HasMany(m => m.Bookings)
                .WithOne(b => b.Member)
                .HasForeignKey(b => b.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.StartTime);

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.FacilityId);

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.MemberId);

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default admin user
            var adminUser = new User
            {
                Email = "admin@sportsfacility.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                FirstName = "Admin",
                LastName = "User",
                Role = "Admin",
                PhoneNumber = "+919876543210",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            modelBuilder.Entity<User>().HasData(adminUser);
        }
    }

}
