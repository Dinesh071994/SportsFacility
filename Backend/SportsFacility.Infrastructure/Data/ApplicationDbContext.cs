using Microsoft.EntityFrameworkCore;
using SportsFacility.Entity.Entities;

namespace SportsFacility.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Facility> Facilities { get; set; } = null!;
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; } = null!;
        public DbSet<UserMembership> UserMemberships { get; set; } = null!;
        public DbSet<UserMembershipDependent> UserMembershipDependents { get; set; } = null!;
        public DbSet<Court> Courts { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<ClassSchedule> ClassSchedules { get; set; } = null!;
        public DbSet<ClassAttendance> ClassAttendances { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Relationships

            // SubscriptionPlan -> Facility
            modelBuilder.Entity<SubscriptionPlan>()
                .HasOne(s => s.Facility)
                .WithMany(f => f.SubscriptionPlans)
                .HasForeignKey(s => s.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserMembership -> User
            modelBuilder.Entity<UserMembership>()
                .HasOne(um => um.User)
                .WithMany(u => u.PrimaryMemberships)
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserMembership -> SubscriptionPlan
            modelBuilder.Entity<UserMembership>()
                .HasOne(um => um.SubscriptionPlan)
                .WithMany(sp => sp.UserMemberships)
                .HasForeignKey(um => um.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserMembershipDependent -> UserMembership
            modelBuilder.Entity<UserMembershipDependent>()
                .HasOne(umd => umd.UserMembership)
                .WithMany(um => um.Dependents)
                .HasForeignKey(umd => umd.UserMembershipId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserMembershipDependent -> User (Dependent)
            modelBuilder.Entity<UserMembershipDependent>()
                .HasOne(umd => umd.DependentUser)
                .WithMany(u => u.DependentMemberships)
                .HasForeignKey(umd => umd.DependentUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Court -> Facility
            modelBuilder.Entity<Court>()
                .HasOne(c => c.Facility)
                .WithMany(f => f.Courts)
                .HasForeignKey(c => c.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> Court
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Court)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CourtId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> User
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClassSchedule -> Facility
            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Facility)
                .WithMany(f => f.ClassSchedules)
                .HasForeignKey(cs => cs.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClassAttendance -> ClassSchedule
            modelBuilder.Entity<ClassAttendance>()
                .HasOne(ca => ca.ClassSchedule)
                .WithMany(cs => cs.Attendances)
                .HasForeignKey(ca => ca.ClassScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClassAttendance -> User
            modelBuilder.Entity<ClassAttendance>()
                .HasOne(ca => ca.User)
                .WithMany(u => u.ClassAttendances)
                .HasForeignKey(ca => ca.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment -> Invoice (1-to-1)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Payment)
                .WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(i => i.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Global Query Filter for Soft Delete
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Facility>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<SubscriptionPlan>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<UserMembership>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Court>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Booking>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ClassSchedule>().HasQueryFilter(x => !x.IsDeleted);
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var baseEntity = (BaseEntity)entityEntry.Entity;
                baseEntity.ModifiedOn = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    baseEntity.CreatedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
