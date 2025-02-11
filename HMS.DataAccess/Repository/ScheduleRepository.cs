using HMS.DataAccess.Data;
using HMS.Entities.Interfaces;
using HMS.Entities.Models;

namespace HMS.DataAccess.Repository
{
    public class ScheduleRepository : BaseRepository<Schedule>,IScheduleRepository
    {
       private readonly HospitalContext _context;
        public ScheduleRepository(HospitalContext _context):base(_context)
        {
            this._context = _context;
        }

        public List<Schedule> GetSchedules()
        {
           List<Schedule> schedules= _context.Schedules.ToList();
            return schedules;
        }
    }
}
