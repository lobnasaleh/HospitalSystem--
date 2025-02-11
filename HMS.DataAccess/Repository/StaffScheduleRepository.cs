using HMS.DataAccess.Data;
using HMS.Entities.Interfaces;
using HMS.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.DataAccess.Repository
{
    public class StaffScheduleRepository :BaseRepository<StaffSchedule>, IStaffScheduleRepository
    {
       private readonly HospitalContext context;
        public StaffScheduleRepository(HospitalContext _context):base(_context)
        {
            context = _context;
        }

        public void AddStaffSchedule(StaffSchedule staffSchedule)
        {
         
                context.StaffSchedules.Add(staffSchedule);

            
        }

        public List<Schedule> getAvailableTimeSlots(int staffId)
        {
            var staffSchedules =  context.StaffSchedules
               .Include(ss => ss.Schedule) 
               .Include(ss => ss.Staff) 
               .Where(ss => ss.StaffId == staffId) 
               .ToList();


            var appointments =  context.Appointments
           .Where(a => a.StaffId == staffId)
           .ToList();


            var availableSchedules = staffSchedules
            .Where(ss => !appointments.Any(appointment =>
                appointment.AppointmentDateTime.Date == ss.Schedule.Date && 
                TimeOnly.FromDateTime(appointment.AppointmentDateTime) >= ss.Schedule.AvailableFrom && 
                TimeOnly.FromDateTime(appointment.AppointmentDateTime) <= ss.Schedule.AvailableTo)) 
            .Select(ss => ss.Schedule) 
            .ToList();

            return availableSchedules;
        }

        public void save()
        {
            context.SaveChanges();
        }
    }
}
