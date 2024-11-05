

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HospitalManagementSystem2.Models
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions <HospitalContext> options):base(options) 
        {
            
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=HospitalSystem;Integrated Security=True ;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<MedicalHistory> MedicalHistories { get; set; }

        public virtual DbSet<Patient> Patients { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }

        public virtual DbSet<StaffSchedule> StaffSchedules { get; set; }

        public virtual DbSet<User> Users { get; set; }
    }
}
