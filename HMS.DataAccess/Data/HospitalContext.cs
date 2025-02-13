

using HMS.Entites.Models;
using HMS.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HMS.DataAccess.Data
{
    public class HospitalContext : IdentityDbContext<ApplicationUser>
    {
        public HospitalContext(DbContextOptions <HospitalContext> options):base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffSchedule>().HasKey(x => new {
            x.StaffId,
            x.ScheduleId
            });//composite key


            base.OnModelCreating(modelBuilder);

            //Table Per Type
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<Patient>().ToTable("Patients");
            modelBuilder.Entity<Staff>().ToTable("Staff");
        }


        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<MedicalHistory> MedicalHistories { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<StaffSchedule> StaffSchedules { get; set; }
        public virtual DbSet<ApplicationUser> Users { get; set; }
    }
}
