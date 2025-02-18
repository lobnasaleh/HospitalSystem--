
using AutoMapper;
using HMS.DataAccess.Data;
using HMS.DataAccess.Repository;
using HMS.DataAccess.Services.AuthService;
using HMS.Entites.Interfaces;
using HMS.Entites.Models;
using HMS.Entities.Interfaces;
using HMS.web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace HMS.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
         

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<HospitalContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"),
                b => b.MigrationsAssembly(typeof(HospitalContext).Assembly.FullName)
                    );//ba2olo ye3ml el migrations folder fel DataAccess l2eno met3wed ye3mlha 3nd el startup project 
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<HospitalContext>();
            //.AddDefaultTokenProviders();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));//auto mapper

        /*    // Debug AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>(); 
            });

            // Validate AutoMapper configuration
            config.AssertConfigurationIsValid();*/

            builder.Services.AddScoped<IAuthService,AuthService>();
        


            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IStaffScheduleRepository, StaffScheduleRepository>();
           // builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
           // builder.Services.AddScoped<IStaffRepository, StaffRepository>();
          //  builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
          //  builder.Services.AddScoped<IMedicalHistoriesRepository, MedicalHistoriesRepository>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
          //  app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
