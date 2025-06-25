using HospitalManagementSystem.Core.Models.Admin;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.DataAccess.Data; // For seed data

namespace HospitalManagementSystem.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<StaffMember> StaffMembers { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Nurse> Nurses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StaffMember>().ToTable("StaffMembers");
            modelBuilder.Entity<Doctor>().ToTable("Doctors");
            modelBuilder.Entity<Nurse>().ToTable("Nurses");

            // Seed initial data (optional but good for testing)
            // SeedData.Seed(modelBuilder);
        }
    }
}