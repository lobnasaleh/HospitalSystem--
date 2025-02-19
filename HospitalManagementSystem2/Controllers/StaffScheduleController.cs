
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
            var currentDate = DateTime.Now.Date; // If ss.Date is DateTime
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            var ass=  await unitOfWork.StaffScheduleRepository.getAllAsync(ss => !ss.IsDeleted
           && (ss.Schedule.Date > currentDate || (ss.Schedule.Date == currentDate && ss.Schedule.AvailableFrom > currentTime))//el hagat ely ye2dar ye3mlha deassign lazem tkoon 

          , new[] {"Staff.Department","Schedule"} );

            return View (ass);
        }


        [HttpGet]
        public async Task< IActionResult> Assign()
        {
            
            var currentDate = DateTime.Now.Date; // If ss.Date is DateTime
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            AssignVM assignVM = new AssignVM()
            {
                Staff= await unitOfWork.StaffRepository.getAllAsync(s=>!s.IsDeleted),
                Schedules=await unitOfWork.ScheduleRepository.getAllAsync(ss => !ss.IsDeleted &&
                (ss.Date > currentDate ||
                (ss.Date == currentDate && ss.AvailableFrom > currentTime)))
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

            //check if this schedule was already assigned to the staff member but marked deleted -->mark it not deleted

            StaffSchedule foundbutedeleted = await unitOfWork.StaffScheduleRepository.getAsync(ss => ss.IsDeleted && ss.StaffId == assignVM.StaffId && ss.ScheduleId == assignVM.ScheduleId);
            if (foundbutedeleted != null)
            {

                foundbutedeleted.IsDeleted = false;
                unitOfWork.StaffScheduleRepository.Update(foundbutedeleted);
                await unitOfWork.completeAsync();

                return RedirectToAction("getAssignedStaff");
            }
            StaffSchedule sc=await unitOfWork.StaffScheduleRepository.getAsync(s=>!s.IsDeleted && s.StaffId==assignVM.StaffId && s.ScheduleId==assignVM.ScheduleId
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

            var sched = await unitOfWork.ScheduleRepository.getAllAsync(s => s.Id == ScheduleId && !s.IsDeleted
          );

            if (!sched.Any()||! asp.Any()) {
                return NotFound();
            
            }

            bool hasConflict = asp.Any(app => sched.Any(s => app.Status==AppointmentStatus.UPCOMING && app.AppointmentDateTime.TimeOfDay == s.AvailableFrom.ToTimeSpan()));

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
