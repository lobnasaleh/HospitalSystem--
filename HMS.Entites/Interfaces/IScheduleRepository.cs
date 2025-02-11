
using HMS.Entites.Interfaces;
using HMS.Entities.Models;

namespace HMS.Entities.Interfaces
{
    public interface IScheduleRepository:IBaseRepository<Schedule>
    {
        public List<Schedule> GetSchedules();
    }
}
