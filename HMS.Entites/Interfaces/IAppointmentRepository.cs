

using HMS.Entites.Interfaces;
using HMS.Entities.Models;

namespace HMS.Entities.Interfaces
{
    public interface IAppointmentRepository:IBaseRepository<Appointment>
    {
        public List<Appointment>GetAllByPatient(int patientId);

        public List<Appointment> GetAllByDoctor(int DoctorId);

        public Appointment GetById(int AppointmentId);

        public void AddAppointment(Appointment appointment);

        public void RemoveAppointment(int Id);

        public void UpdateAppointment(Appointment appointment);

        public List<Staff> GetAllStaff(int DepartmentId);
        public void Save();
    }
}
