using HospitalManagementSystem2.Models;

namespace HospitalManagementSystem2.Repository.Interfaces
{
    public interface IScheduleRepository
    {
        public List<Schedule> GetSchedules();
    }
}
