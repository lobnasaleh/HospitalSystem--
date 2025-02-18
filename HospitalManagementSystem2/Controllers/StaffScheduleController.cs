
using AutoMapper;
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

    /*    [HttpGet]
        public async Task<IActionResult> AssignedStaff()
        {

        }*/


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
        public async Task<IActionResult> SaveAssign(AssignVM assignVM)
        {
            if (!ModelState.IsValid) { 

            return View(assignVM);
            }

            StaffSchedule sc=await unitOfWork.StaffScheduleRepository.getAsync(s=>s.StaffId==assignVM.StaffId && s.ScheduleId==assignVM.ScheduleId
            && !s.Staff.IsDeleted && !s.Schedule.IsDeleted, true, new[] {"Schedule","Staff"});

            if (sc != null) {

                TempData["Error"] = "This Schedule is already assigned to This Staff Member";
                return RedirectToAction("Assign");
            }

            //map assignvm to staffschedule
             StaffSchedule ss=mapper.Map<StaffSchedule>(sc);
            await unitOfWork.StaffScheduleRepository.AddAsync(ss);
            await unitOfWork.completeAsync();

            return View();
        }


    }
}
