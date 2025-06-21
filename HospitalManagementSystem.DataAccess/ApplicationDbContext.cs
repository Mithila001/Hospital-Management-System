using HospitalManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.DataAccess.Data; // For seed data

namespace HospitalManagementSystem.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<StaffMember> StaffMembers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure StaffMember
            modelBuilder.Entity<StaffMember>()
                .HasIndex(s => s.Username) // Ensure username is unique for staff members
                .IsUnique();

            // Seed initial data (optional but good for testing)
            // SeedData.Seed(modelBuilder);
        }
    }
}