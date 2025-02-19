
using AutoMapper;
using HMS.Entites.Enums;
using HMS.Entites.Interfaces;
using HMS.Entites.ViewModel;
using HMS.Entities.Interfaces;
using HMS.Entities.Models;
using Microsoft.AspNetCore.Mvc;


namespace HMS.web.Controllers
{
    public class StaffScheduleController : Controller

    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public StaffScheduleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> getAssignedStaff()
        {
          var ass=  await unitOfWork.StaffScheduleRepository.getAllAsync(ss => !ss.IsDeleted, new[] {"Staff.Department","Schedule"} );

            return View (ass);
        }


        [HttpGet]
        public async Task< IActionResult> Assign()
        {
            AssignVM assignVM = new AssignVM()
            {
                Staff= await unitOfWork.StaffRepository.getAllAsync(s=>!s.IsDeleted),
                Schedules=await unitOfWork.ScheduleRepository.getAllAsync(ss=>!ss.IsDeleted)
            };

            return View(assignVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AssignVM assignVM)
        {
            if (!ModelState.IsValid) {
                //refilling the form
                AssignVM ass = new AssignVM()
                {
                    Staff = await unitOfWork.StaffRepository.getAllAsync(s => !s.IsDeleted),
                    Schedules = await unitOfWork.ScheduleRepository.getAllAsync(ss => !ss.IsDeleted)
                };
                return View(ass);
            }

            StaffSchedule sc=await unitOfWork.StaffScheduleRepository.getAsync(s=>s.StaffId==assignVM.StaffId && s.ScheduleId==assignVM.ScheduleId
            && !s.Staff.IsDeleted && !s.Schedule.IsDeleted, true, new[] {"Schedule","Staff"});

            if (sc != null) {

                TempData["Error"] = "This Schedule is already assigned to This Staff Member";
                return RedirectToAction("Assign");
            }

            //map assignvm to staffschedule
            // StaffSchedule ss=mapper.Map<StaffSchedule>(sc);
            StaffSchedule ss = new StaffSchedule()
            {
                ScheduleId = assignVM.ScheduleId,
                StaffId = assignVM.StaffId
            };

            await unitOfWork.StaffScheduleRepository.AddAsync(ss);
            await unitOfWork.completeAsync();

            return RedirectToAction("getAssignedStaff");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDeAssign(int ScheduleId ,string StaffId)
        {
            var s = await unitOfWork.StaffScheduleRepository.getAsync(ss =>!ss.IsDeleted && ss.ScheduleId == ScheduleId && ss.StaffId == StaffId);
            if (s == null)
            {
                return NotFound();
            }
            return View("DeAssign", s);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeAssign(string StaffId, int ScheduleId)
        {
        StaffSchedule ss = await unitOfWork.StaffScheduleRepository.getAsync(s => !s.IsDeleted && s.StaffId == StaffId && s.ScheduleId == ScheduleId 
        && !s.Schedule.IsDeleted && !s.Staff.IsDeleted, true,new[] {"Staff","Schedule"}
        
        );
            if (ss == null) { 
            
            return NotFound();
            }
            
            //can not deassign a schedule that is already an appointment
             var asp=   await unitOfWork.AppointmentRepository.
                getAllAsync(a => a.Status==AppointmentStatus.CANCELLED && a.StaffId == StaffId);

            var sched = await unitOfWork.ScheduleRepository.getAllAsync(s => s.Id == ScheduleId && !s.IsDeleted);

            if (!sched.Any()||! asp.Any()) {
                return NotFound();
            
            }

            bool hasConflict = asp.Any(app => sched.Any(s => app.AppointmentDateTime.TimeOfDay == s.AvailableFrom.ToTimeSpan()));

            if (hasConflict) {
                TempData["Error"] = "Can not Deassign a Schedule that is an Appointment";
                return RedirectToAction("getAssignedStaff");
            }

            ss.IsDeleted = true;
          unitOfWork.StaffScheduleRepository.Update(ss);
          await unitOfWork.completeAsync();
            return RedirectToAction("getAssignedStaff");


        }

    }
}
