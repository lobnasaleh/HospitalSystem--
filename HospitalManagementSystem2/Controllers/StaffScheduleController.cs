using HospitalManagementSystem2.Models;
using HospitalManagementSystem2.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem2.Controllers
{
    public class StaffScheduleController : Controller

    {
        private readonly IStaffScheduleRepository staffScheduleRepository;
        private readonly IStaffRepository staffRepository;
        private readonly IScheduleRepository scheduleRepository;


        public StaffScheduleController(IStaffRepository _StaffRepository, IScheduleRepository _schedulRepository, IStaffScheduleRepository _staffScheduleRepository)
        {
            staffRepository = _StaffRepository;
            scheduleRepository = _schedulRepository;
            staffScheduleRepository = _staffScheduleRepository;
        }


        //StaffSchedule/Assign
        public IActionResult Assign()
        {
            List<Staff> staff = staffRepository.GetStaff();
            ViewBag.Staff = staff;

            List<Schedule> schedules = scheduleRepository.GetSchedules();

            var scheduleList = schedules.Select(s => new
            {
                Id = s.Id,
                DisplayText = $"{s.AvailableFrom:hh:mm tt} - {s.AvailableTo:hh:mm tt} on {s.Date:MM/dd/yyyy}"
            }).ToList();

            ViewBag.Schedules = scheduleList;

            return View();
        }

        //StaffSchedule/SaveAssign
        public IActionResult SaveAssign(StaffSchedule staffSchedule)
        {
            List<Staff> staff = staffRepository.GetStaff();
            ViewBag.Staff = staff;
            List<Schedule> schedules = scheduleRepository.GetSchedules();
            var scheduleList = schedules.Select(s => new
            {
                Id = s.Id,
                DisplayText = $"{s.AvailableFrom:hh:mm tt} - {s.AvailableTo:hh:mm tt} on {s.Date:MM/dd/yyyy}"
            }).ToList();
            ViewBag.Schedules = scheduleList;

            if (ModelState.IsValid)
            {
                if (staffSchedule.StaffId != 0 && staffSchedule.ScheduleId != 0)//satffid and scheduled not sent from form with 0
                {
                    staffScheduleRepository.AddStaffSchedule(staffSchedule);
                    staffScheduleRepository.save();
                    TempData["SuccessMessage"] = "Schedule Assigned Successfully";
                    return View("Assign");
                }
               else if (staffSchedule.StaffId == 0 && staffSchedule.ScheduleId == 0)
                {
                    ModelState.AddModelError("ScheduleId", "Please Select A Schedule");
                    ModelState.AddModelError("StaffId", "Please Select A Staff Member");

                    return View("Assign");
                }

                else if (staffSchedule.ScheduleId == 0)
                {
                    ModelState.AddModelError("ScheduleId", "Please Select A Schedule");
                    return View("Assign");
                }
                else
                {//staff id =0
                    ModelState.AddModelError("StaffId", "Please Select A Staff Member");

                    return View("Assign");

                }

            }
            return View("Assign");
        }


    }
}
