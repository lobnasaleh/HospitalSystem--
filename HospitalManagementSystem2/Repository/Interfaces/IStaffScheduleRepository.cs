using HospitalManagementSystem2.Models;

namespace HospitalManagementSystem2.Repository.Interfaces
{
    public interface IStaffScheduleRepository
    {
        void AddStaffSchedule(StaffSchedule staffSchedule);
        public List<Schedule> getAvailableTimeSlots(int staffid);

        public void save();


    }
}
