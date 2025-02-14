
using AutoMapper;
using HMS.DataAccess.Repository;
using HMS.Entites.Interfaces;
using HMS.Entites.ViewModel;
using HMS.Entities.Interfaces;
using HMS.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.web.Controllers
{
    public class DepartmentController : Controller
    {
       private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        //Department/GetAllDepartments
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {

            var departments = await unitOfWork.DepartmentRepository.getAllAsync(d=>!d.IsDeleted);
            return View(departments);
        }
        //Department/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DepartmentViewModel departmentvm)
        {

            if (ModelState.IsValid)
            {

               Department depp= await unitOfWork.DepartmentRepository.getAsync(d=>d.Name == departmentvm.Name,false);
                if (depp != null)
                {
                    ModelState.AddModelError("Name", "A Department with this name already exists.");
                    return View(departmentvm);
                }

                Department dept = mapper.Map<Department>(departmentvm);
                await unitOfWork.DepartmentRepository.AddAsync(dept);
                await unitOfWork.completeAsync();
                return RedirectToAction("GetAllDepartments");
            }
            return View();
        }
        [HttpGet]
        public async Task< IActionResult> GetDepartmentById(int id)
        {

            var department = await unitOfWork.DepartmentRepository.getAsync(d=>!d.IsDeleted && d.Id==id);
            if (department == null) { 
            
              return NotFound();
            }
            return View("SpecificDepartment",department);
        }
        //Department/Update/1
        [HttpGet]
        public async Task<IActionResult> UpdateDepartment(int id)
        {
            var department = await unitOfWork.DepartmentRepository.getAsync(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }
           var toupdate= mapper.Map<DepartmentViewModel>(department);
            return View( toupdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDepartment(int id,DepartmentViewModel newdeptvm)
        {
            if (ModelState.IsValid)
            {
               Department d= await unitOfWork.DepartmentRepository.getAsync(d=>d.Id==id);
                if (d is null)
                {
                    return NotFound();
                }

                d.Name = newdeptvm.Name;
                unitOfWork.DepartmentRepository.Update(d);
                await unitOfWork.completeAsync();
                return RedirectToAction("GetAllDepartments");

            }
            return View(newdeptvm);
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var department = await unitOfWork.DepartmentRepository.getAsync(ss => ss.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            return View("Delete", department);

        }
        //Department/DeleteDepartment/1

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
           
                Department d = await unitOfWork.DepartmentRepository.getAsync(d => d.Id == id);
                if (d is null)
                {
                    return NotFound();
                }
                //check if department is not assigned to any appointment or has staff members

              Staff staff=  await unitOfWork.StaffRepository.getAsync(s=>s.DepartmentId==id);
              Appointment appointment = await unitOfWork.AppointmentRepository.getAsync(a=>a.DepartmentId==id && a.AppointmentDateTime>=DateTime.Today);
              if (appointment is not null || staff is not null)
              {
                TempData["ErrorMessage"] = "Can not delete a Department having upcoming appointments or assigned staff";
                    return RedirectToAction("GetAllDepartments"); }

                d.IsDeleted = true;
                unitOfWork.DepartmentRepository.Update(d);
                await unitOfWork.completeAsync();
                return RedirectToAction("GetAllDepartments");
        }

 
    }
}
