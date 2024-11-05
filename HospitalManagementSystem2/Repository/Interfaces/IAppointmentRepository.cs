using HospitalManagementSystem2.Models;

namespace HospitalManagementSystem2.Repository.Interfaces
{
    public interface IAppointmentRepository
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
