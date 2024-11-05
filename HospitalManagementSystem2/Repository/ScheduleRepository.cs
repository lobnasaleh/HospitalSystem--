using HospitalManagementSystem2.Models;
using HospitalManagementSystem2.Repository.Interfaces;

namespace HospitalManagementSystem2.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        HospitalContext context;
        public ScheduleRepository(HospitalContext _context)
        {
            context = _context;
        }

        public List<Schedule> GetSchedules()
        {
           List<Schedule> schedules= context.Schedules.ToList();
            return schedules;
        }
    }
}
