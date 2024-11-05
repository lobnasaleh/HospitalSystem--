using HospitalManagementSystem2.Models;
using HospitalManagementSystem2.Repository;
using HospitalManagementSystem2.Repository.Interfaces;
using HospitalManagementSystem2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagementSystem2.Controllers
{
    public class AppointmentController : Controller
    {
        IAppointmentRepository appointmentRepository;
        IStaffScheduleRepository scheduleRepository;
        IDepartmentRepository departmentRepository;

        public AppointmentController(IAppointmentRepository _appointmnetrepo, IStaffScheduleRepository _scheduleRepository, IDepartmentRepository _departmentRepository)
        {
           appointmentRepository= _appointmnetrepo;
            scheduleRepository= _scheduleRepository;
            departmentRepository= _departmentRepository;
        }
        //Appointment/Test
        public IActionResult Test() { 
          return View();
        }

        //Appointment/GetAppointmentsByPatient?PatientId=
        public IActionResult GetAppointmentsByPatient(int PatientId)
        {
            
            List <Appointment> appointments=appointmentRepository.GetAllByPatient(PatientId);

            return View("ViewAppointments",appointments);
        }

        //Appointment/GetAppointmentsByDoctor?StaffId=
        public IActionResult GetAppointmentsByDoctor(int StaffId)
        {

            List<Appointment> appointments = appointmentRepository.GetAllByDoctor(StaffId);

            return View("ViewAppointments", appointments);
        }

        //Appointment/GetAppointmentById/
        
        public IActionResult GetAppointmentById(int Id)
        {

            Appointment appointment = appointmentRepository.GetById(Id);

            return View("SpecificAppointment", appointment);
        }
        //Appointment/GetAvailableTimeSlots?StaffId=1
        public IActionResult GetAvailableTimeSlots(int Id, int DepartmentId)//
        {
            ViewBag.StaffId = Id;
            ViewBag.DepartmentId = DepartmentId;
            List<Schedule> schedule = scheduleRepository.getAvailableTimeSlots(Id);

            return View("DoctorAvailableTimeSlots", schedule);
        }

        //Appointment/GetAllDepartments
        public IActionResult GetAllDepartments()
        {

            var departmentList = departmentRepository.GetAllDepartments();
            ViewBag.Departments = new SelectList(departmentList, "Id", "Name");
            return View("ChooseDepartment");
        }

        public IActionResult Delete(int Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                appointmentRepository.RemoveAppointment(Id);
                appointmentRepository.Save();
            }
          return RedirectToAction("PUser", "PUser");
        }

        //Appointment/ChooseDoctor
        public IActionResult ChooseDoctor(int Id)
        {

            List<Staff> staff = appointmentRepository.GetAllStaff(Id);
            return View(staff);
        }

        public IActionResult BookAppointment(Appointment appointment)
        {
            appointment.Status = "Scheduled";
            appointmentRepository.AddAppointment(appointment);
            appointmentRepository.Save();
            return RedirectToAction("GetAppointmentsByPatient", new { PatientId = appointment.PatientId });

        }

        public IActionResult UpdateAppointment(int Id,int DepartmentId )
        {
            ViewBag.Id = Id;
            ViewBag.DepartmentId = DepartmentId;
            return View();
        }

        public IActionResult DeleteAndChange(int Id,int DepartmentId)
        {
            appointmentRepository.RemoveAppointment(Id);
            appointmentRepository.Save();
            List<Staff> staff = appointmentRepository.GetAllStaff(DepartmentId);
            return View("ChooseDoctor", staff);
        }

    }
}
