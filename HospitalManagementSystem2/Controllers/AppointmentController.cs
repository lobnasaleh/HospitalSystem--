
using AutoMapper;
using HMS.DataAccess.Migrations;
using HMS.Entites.Enums;
using HMS.Entites.Interfaces;
using HMS.Entites.ViewModel;
using HMS.Entities.Interfaces;
using HMS.Entities.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllAppointments()
        {
            var a = await unitOfWork.AppointmentRepository.getAllAsync(null, new[] { "Department", "Patient", "Staff" });
            return View(a);
        }

        public async Task<IActionResult> AppointmentOfDoc()
        { //to show it for doctor writing histories 
            //get the id of the logged in staff 
            string docid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Appointment> appointments = await unitOfWork.AppointmentRepository
                  .getAllAsync(a => a.Status != AppointmentStatus.CANCELLED && a.StaffId == docid, new[] { "Department", "Patient" }
                  );//ma ansash a3adelha

            return View("AppointmentOfDoctor", appointments);
        }


        //doctor available appointments
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> getAvaialbleAppointmentsOfDoctor(int DepartmentId, string StaffId)//to show it for patient
        {
            ViewBag.PatientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentDate = DateTime.Now.Date; // If ss.Date is DateTime
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            ViewBag.StaffId = StaffId;
            ViewBag.DepartmentId = DepartmentId;

            var appointments = await unitOfWork.AppointmentRepository
               .getAllAsync(a => a.Status != AppointmentStatus.CANCELLED && a.StaffId == StaffId, new[] { "Department" });

            var staffSchedules = await unitOfWork.StaffScheduleRepository.getAllAsync(ss => !ss.IsDeleted && !ss.Schedule.IsDeleted && !ss.Staff.IsDeleted && ss.StaffId == StaffId
             && (ss.Schedule.Date > currentDate || (ss.Schedule.Date == currentDate && ss.Schedule.AvailableFrom > currentTime))//el hagat ely ye2dar ye3mlha deassign lazem tkoon 
            , new[] { "Schedule", "Staff" });

            if (!staffSchedules.Any())
            {
                TempData["Error"] = "No Available Appointments";
                return RedirectToAction("Search", "Staff");
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
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> BookAppointment(BookAppointmentVM bookAppointmentVM)//to show it for patient
        {

            if (bookAppointmentVM == null || !ModelState.IsValid)
            {
                TempData["Error"] = "Invalid appointment details!";
                return RedirectToAction("getAvaialbleAppointmentsOfDoctor", new { bookAppointmentVM.DepartmentId, bookAppointmentVM.StaffId });

            }
            Appointment ap = mapper.Map<Appointment>(bookAppointmentVM);
            ap.Status = AppointmentStatus.UPCOMING;
            await unitOfWork.AppointmentRepository.AddAsync(ap);
            if (await unitOfWork.completeAsync() <= 0)
            {
                //fail
                TempData["Error"] = "Could not book your Appointment";
                return RedirectToAction("getAvaialbleAppointmentsOfDoctor", new { bookAppointmentVM.DepartmentId, bookAppointmentVM.StaffId });

            }

            return RedirectToAction("AppointmentsOfPatient"); //success
        }

        //getAppointmentsByPatient-->patient
        [Authorize(Roles = "Patient")]

        public async Task<IActionResult> AppointmentsOfPatient()
        {
            //get the id of the logged in patient
            string patientid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Appointment> appointments = await unitOfWork.AppointmentRepository
                  .getAllAsync(a => a.PatientId == patientid, new[] { "Department", "Staff" }
                  );//ma ansash a3adelha

            return View("AppointmentsOfPatient", appointments);
        }

        //CANCEL APPOINTMENT
        [HttpGet]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> ConfirmDelete(int id)//appointmentid
        {
            var appointment = await unitOfWork.AppointmentRepository.getAsync(a => a.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View("Delete", appointment);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Patient")]

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


    }
}
