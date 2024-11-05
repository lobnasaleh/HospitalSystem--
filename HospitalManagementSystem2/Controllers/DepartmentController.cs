using HospitalManagementSystem2.Models;
using HospitalManagementSystem2.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem2.Controllers
{
    public class DepartmentController : Controller
    {
        IDepartmentRepository departmentRepository;

        public DepartmentController(IDepartmentRepository _departmentRepository)
        {
            departmentRepository = _departmentRepository;
        }

        //Department/GetAllDepartments
        public IActionResult GetAllDepartments()
        {

            var departments = departmentRepository.GetAllDepartments();
            return View(departments);
        }
        //Department/Add
        public IActionResult Add()
        {
            return View();
        }

        public IActionResult SaveNew(Department department)
        {
            department.IsDeleted = false;
            if (ModelState.IsValid)
            {
                departmentRepository.AddDepartment(department);
                departmentRepository.Save();
                return RedirectToAction("GetAllDepartments");
            }
            return View("Add");
        }
    }
}
