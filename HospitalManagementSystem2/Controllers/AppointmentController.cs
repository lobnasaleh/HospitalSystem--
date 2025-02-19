
using AutoMapper;
using HMS.DataAccess.Migrations;
using HMS.Entites.Enums;
using HMS.Entites.Interfaces;
using HMS.Entites.ViewModel;
using HMS.Entities.Interfaces;
using HMS.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace HMS.web.Controllers
{
    public class AppointmentController : Controller
    {

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public AppointmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        //admin
        public async Task<IActionResult> AllAppointments()
        {
           var a= await unitOfWork.AppointmentRepository.getAllAsync(null, new[] { "Department", "Patient" ,"Staff"});
            return View(a);
        }

        public async Task<IActionResult> AppointmentOfDoc() { //to show it for doctor writing histories 
            //get the id of the logged in staff 
           // string docid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
          IEnumerable<Appointment> appointments=  await unitOfWork.AppointmentRepository
                .getAllAsync(a=>a.Status!=AppointmentStatus.CANCELLED && a.StaffId== "1a2b3c4d-1234-5678-90ab-cdef12345678", new[] { "Department", "Patient" } 
                );//ma ansash a3adelha

         return View("AppointmentOfDoctor",appointments);
        }


        //doctor available appointments
        public async Task<IActionResult> getAvaialbleAppointmentsOfDoctor(int DepartmentId,string StaffId)//to show it for patient
        {
            ViewBag.StaffId = StaffId;
            ViewBag.DepartmentId = DepartmentId;

            var appointments = await unitOfWork.AppointmentRepository
               .getAllAsync(a => a.Status!=AppointmentStatus.CANCELLED && a.AppointmentDateTime> DateTime.Now && a.StaffId == StaffId, new[] { "Department" });

            var staffSchedules = await unitOfWork.StaffScheduleRepository.getAllAsync(ss => !ss.IsDeleted &&!ss.Schedule.IsDeleted && !ss.Staff.IsDeleted && ss.StaffId == StaffId, new[] { "Schedule","Staff" });

            if (!staffSchedules.Any()) {
                TempData["Error"] = "No Available Appointments";
                return RedirectToAction("Search","Staff");
            }

            var availableSchedules = staffSchedules
            .Where(ss => !appointments.Any(appointment =>
                appointment.AppointmentDateTime.Date == ss.Schedule.Date &&
                TimeOnly.FromDateTime(appointment.AppointmentDateTime) >= ss.Schedule.AvailableFrom &&
                TimeOnly.FromDateTime(appointment.AppointmentDateTime) <= ss.Schedule.AvailableTo))
            .Select(ss => ss.Schedule)
            .ToList();
            

            return View(availableSchedules);
        }
        //Book Appointment
        public async Task<IActionResult> BookAppointment(BookAppointmentVM bookAppointmentVM)//to show it for patient
        {

            if (bookAppointmentVM == null)
            {
                return NotFound(); // Handle the case where the model is not properly bound
            }
            Appointment ap=  mapper.Map<Appointment>(bookAppointmentVM);
            ap.Status=AppointmentStatus.UPCOMING;
            await unitOfWork.AppointmentRepository.AddAsync(ap);
            if (  await unitOfWork.completeAsync() > 1)
            {
                //fail
                TempData["Error"] = "Could not book your Appointment";
                return RedirectToAction("getAvaialbleAppointmentsOfDoctor", new { bookAppointmentVM.DepartmentId, bookAppointmentVM.StaffId });

            }

            return RedirectToAction("AppointmentsOfPatient"); //success
        }

        //getAppointmentsByPatient-->patient

        public async Task<IActionResult> AppointmentsOfPatient()
        {
            //get the id of the logged in patient
            // string patientid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Appointment> appointments = await unitOfWork.AppointmentRepository
                  .getAllAsync(a => a.PatientId == "7E596CCF-CCA2-480C-B830-BBB6513D7309", new[] { "Department", "Staff" }
                  );//ma ansash a3adelha

            return View("AppointmentsOfPatient", appointments);
        }

        //CANCEL APPOINTMENT
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)//appointmentid
        {
            var appointment = await unitOfWork.AppointmentRepository.getAsync(a=> a.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View("Delete", appointment);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

           Appointment a = await unitOfWork.AppointmentRepository.getAsync(d => d.Id == id);
            if (a is null)
            {
                return NotFound();
            }
            if ((a.AppointmentDateTime - DateTime.Now).TotalHours < 24)
            {
                TempData["Error"] = "You can only cancel an appointment at least 24 hours in advance.";
                return RedirectToAction("AppointmentsOfPatient");

            }

            a.Status = AppointmentStatus.CANCELLED;
            unitOfWork.AppointmentRepository.Update(a);
            await unitOfWork.completeAsync();
            return RedirectToAction("AppointmentsOfPatient");
        }








        //get AppointmentById -->patient




























        /*    IAppointmentRepository appointmentRepository;
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
                //aseebha tanyy please
              *//*  var departmentList = departmentRepository.GetAllDepartments();
                ViewBag.Departments = new SelectList(departmentList, "Id", "Name");
               *//* return View("ChooseDepartment");
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
            }*/

    }
}
