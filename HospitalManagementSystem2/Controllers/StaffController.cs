using AutoMapper;
using HMS.DataAccess.Repository;
using HMS.Entites.Contracts;
using HMS.Entites.Enums;
using HMS.Entites.Interfaces;
using HMS.Entites.ViewModel;
using HMS.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HMS.web.Controllers
{
    public class StaffController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        private readonly IAuthService _authService;

        public StaffController(IUnitOfWork _unitOfWork, IMapper mapper, IAuthService _authService)
        {
            this._unitOfWork = _unitOfWork;
            this.mapper = mapper;
            this._authService = _authService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var st = await _unitOfWork.StaffRepository.getAllAsync(s => !s.IsDeleted, new[] {"Department"} );
            return View(st);

        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var depts=await _unitOfWork.DepartmentRepository.getAllAsync(d=>!d.IsDeleted);
            var departmentList = depts.Select(d => new SelectListItem
            {
                Value=d.Id.ToString(),
                Text=d.Name
            });
            ViewBag.Departments = departmentList;

            var positions = Enum.GetValues(typeof(Position))
                    .Cast<Position>()
                    .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() });

            ViewBag.Positions =positions;

            return View();
        }

        [HttpPost]
        //admin only
        public async Task<IActionResult> Create(RegisterStaffRequestVM staffFromReq)
        {
     
            if (ModelState.IsValid)
            {

                Staff staff = await _unitOfWork.StaffRepository.getAsync(s => !s.IsDeleted && (s.Email==staffFromReq.Email || s.UserName==staffFromReq.UserName), false);
                if (staff != null)
                {

                    ModelState.AddModelError("Name", "A staff member with this Email or Username already exists.");
                    return View(staffFromReq);
                }
                
                var st = mapper.Map<RegisterStaffRequest>(staffFromReq);
                await _authService.RegisterStaff(st);//need to handle the token
               
                return RedirectToAction("Index");
            }
            //to refill selects
            var depts = await _unitOfWork.DepartmentRepository.getAllAsync(d => !d.IsDeleted);
            var departmentList = depts.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });
            ViewBag.Departments = departmentList;
            var positions = Enum.GetValues(typeof(Position))
                               .Cast<Position>()
                               .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() });

            ViewBag.Positions = positions;

            return View(staffFromReq);
        }
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {

           Staff st= await _unitOfWork.StaffRepository.getAsync(s => s.Id == id,false);
            if (st == null) { 
              return NotFound();
            }

            RegisterStaffRequestVM staffmp = new RegisterStaffRequestVM()
            {
                PasswordHash = st.PasswordHash,
                FullName = st.FullName,
                Email = st.Email,
                Position = (int)st.Position,
                Address = st.Address,
                DepartmentId = st.DepartmentId,
                Qualification = st.Qualification,
                UserName = st.UserName

            };//from staff to staffmp


            var depts = await _unitOfWork.DepartmentRepository.getAllAsync(d => !d.IsDeleted);
            var departmentList = depts.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });
            ViewBag.Departments = departmentList;

            var positions = Enum.GetValues(typeof(Position))
                              .Cast<Position>()
                              .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() });

            ViewBag.Positions = positions;

            return View(staffmp);
        }

        [HttpPost]
        //admin or staff
        public async Task<IActionResult> Update(string id,RegisterStaffRequestVM staffFromReq)
        {
            if (ModelState.IsValid)
            {

                Staff staff = await _unitOfWork.StaffRepository.getAsync(s => !s.IsDeleted && (s.UserName == staffFromReq.UserName), false);
                if (staff != null)
                {

                    ModelState.AddModelError("Email", "A staff member with this Email or Username already exists.");
                    return View(staffFromReq);
                }
                var res= await _authService.UpdateStaffProfile(id, staffFromReq);
                if (res.IsSuccess) { 
                return RedirectToAction("Index");

                }

            }
            //to refill selects
            var depts = await _unitOfWork.DepartmentRepository.getAllAsync(d => !d.IsDeleted);
            var departmentList = depts.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });
            ViewBag.Departments = departmentList;
            var positions = Enum.GetValues(typeof(Position))
                               .Cast<Position>()
                               .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() });

            ViewBag.Positions = positions;

            return View(staffFromReq);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
          /*  Staff s = await _unitOfWork.StaffRepository.getAsync(s=>s.Id==id);
            if (s is null)
            {
                return NotFound();
            }
            //check if Staff is not assigned to any appointments in dates greater than today's date

            Staff staff = await _unitOfWork.StaffRepository.getAsync(s => s.DepartmentId == id);
            Appointment appointment = await unitOfWork.AppointmentRepository.getAsync(a => a.DepartmentId == id && a.AppointmentDateTime >= DateTime.Today);
            if (appointment is not null || staff is not null)
            {
                return BadRequest("Can not delete a Department having upcoming appointments or assigned staff");
            }

            d.IsDeleted = true;
            unitOfWork.DepartmentRepository.Update(d);
            await unitOfWork.completeAsync();*/
            return RedirectToAction("GetAllDepartments");
        }







    }
}
