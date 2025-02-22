using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HMS.DataAccess.Data;
using HMS.Entities.Models;
using HMS.Entites.Interfaces;
using AutoMapper;
using HMS.Entites.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace HMS.web.Controllers
{
    public class ScheduleController : Controller
    {



        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ScheduleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            var s = await unitOfWork.ScheduleRepository.getAllAsync(s => !s.IsDeleted);
            return View(s);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create(ScheduleVM scheduleVM)
        {
            if (!ModelState.IsValid)
            {
                return View(scheduleVM);
            }
            Schedule s = await unitOfWork.ScheduleRepository.getAsync(s =>
            !s.IsDeleted &&
            s.AvailableFrom == scheduleVM.AvailableFrom &&
            s.AvailableTo == scheduleVM.AvailableTo &&
            s.Date == scheduleVM.Date
            );
            if (s != null)
            {

                ModelState.AddModelError("Date", "This Schedule already exists");
                return View(scheduleVM);

            }
            if (scheduleVM.Date < DateTime.Today)
            {
                ModelState.AddModelError("Date", "Schedule Date should be greater than today's date ");
                return View(scheduleVM);
            }
            if (scheduleVM.AvailableFrom >= scheduleVM.AvailableTo)
            {
                ModelState.AddModelError("AvailableFrom", " Start time should be greater than End Time");

                return View(scheduleVM);
            }

            Schedule ss = mapper.Map<Schedule>(scheduleVM);
            await unitOfWork.ScheduleRepository.AddAsync(ss);
            await unitOfWork.completeAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(int id)
        {

            Schedule s = await unitOfWork.ScheduleRepository.getAsync(s => !s.IsDeleted && s.Id == id, false);
            if (s == null)
            {
                return NotFound();
            }

            //map schedule yo schedulevm
            ScheduleVM ss = mapper.Map<ScheduleVM>(s);
            return View(ss);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(int id, ScheduleVM scheduleVM)
        {
            if (ModelState.IsValid)
            {
                Schedule s = await unitOfWork.ScheduleRepository.getAsync(s => !s.IsDeleted && s.Id == id, false);
                if (s == null)
                {
                    return NotFound();
                }
                Schedule sss = await unitOfWork.ScheduleRepository.getAsync(s =>
                 s.AvailableFrom == scheduleVM.AvailableFrom &&
                 s.AvailableTo == scheduleVM.AvailableTo &&
                 s.Date == scheduleVM.Date, false);
                if (sss != null)
                {

                    ModelState.AddModelError("Date", "This Schedule already exists");
                    return View(scheduleVM);

                }
                if (scheduleVM.Date < DateTime.Today)
                {
                    ModelState.AddModelError("Date", "Schedule Date should be greater than today's date");
                    return View(scheduleVM);
                }
                if (scheduleVM.AvailableFrom >= scheduleVM.AvailableTo)
                {
                    ModelState.AddModelError("AvailableFrom", " Start time should be greater than End Time");

                    return View(scheduleVM);
                }

                //check if  available from > available to invalid or date <today

                //map scheduleVm to schedule
                Schedule ss = mapper.Map<Schedule>(scheduleVM);//agarb asheel el assignment
                ss.Id = id;
                unitOfWork.ScheduleRepository.Update(ss);
                await unitOfWork.completeAsync();
                return RedirectToAction("Index");
            }
            return View(scheduleVM);

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var schedule = await unitOfWork.ScheduleRepository.getAsync(ss => ss.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }
            return View("Delete", schedule);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {
            Schedule schd = await unitOfWork.ScheduleRepository.getAsync(s => !s.IsDeleted && s.Id == id);
            if (schd is null)
            {
                return NotFound();
            }
            //is this schedule assigned to a staff 
            StaffSchedule stsc = await unitOfWork.StaffScheduleRepository.getAsync(ss => ss.ScheduleId == id);
            if (stsc != null)
            {

                TempData["ErrorMessage"] = "can not delete this schedule as it is already assigned to a staff member.";
                return RedirectToAction("Index");

                //can not delete this schedule as it is already assigned to a staff member
            }
            schd.IsDeleted = true;
            unitOfWork.ScheduleRepository.Update(schd);
            await unitOfWork.completeAsync();
            TempData["ErrorMessage"] = "Schedule deleted Successfully";

            return RedirectToAction("Index");

        }


    }
}

