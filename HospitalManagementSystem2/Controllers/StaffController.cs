using AutoMapper;
using HMS.DataAccess.Repository;
using HMS.Entites.Contracts;
using HMS.Entites.Enums;
using HMS.Entites.Interfaces;
using HMS.Entites.ViewModel;
using HMS.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            var st = await _unitOfWork.StaffRepository.getAllAsync(s => !s.IsDeleted, new[] {"Department"} );
            return View(st);

        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

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

            ViewBag.Positions = positions;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

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
              var res=  await _authService.RegisterStaff(st);//need to handle the token

                if (!res.isAuthenticated)
                {
                    var depts2 = await _unitOfWork.DepartmentRepository.getAllAsync(d => !d.IsDeleted);
                    var departmentList2 = depts2.Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    });
                    ViewBag.Departments = departmentList2;
                    var positions2 = Enum.GetValues(typeof(Position))
                                       .Cast<Position>()
                                       .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() });

                    ViewBag.Positions = positions2;
                    ModelState.AddModelError("",res.Message);
                    return View(staffFromReq);
                }
               
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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(string id)
        {
            //getting the logged in user 
           // string loggedinuser = User.FindFirstValue(ClaimTypes.NameIdentifier);


            Staff st = await _unitOfWork.StaffRepository.getAsync(s => !s.IsDeleted && s.Id == id, false);
            if (st == null) { 
              return NotFound();
            }

            UpateStaffRequestVM staffmp = new UpateStaffRequestVM()
            {
                FullName = st.FullName,
                Position = (int)st.Position,
                Address = st.Address,
                DepartmentId = st.DepartmentId,
                Qualification = st.Qualification

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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        //admin or staff
        public async Task<IActionResult> Update(string id,UpateStaffRequestVM staffFromReq)//el id da kan fel get howa ehtafz beeh bdoon hiddenfield 
        {
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
            if (ModelState.IsValid)
            {

                //var sttt = mapper.Map<UpateStaffRequest>(staffFromReq);
                UpateStaffRequest up=new UpateStaffRequest()
                {
                    Address = staffFromReq.Address,
                    DepartmentId = staffFromReq.DepartmentId,
                    FullName = staffFromReq.FullName,
                    Position = staffFromReq.Position,
                    Qualification = staffFromReq.Qualification
                   
                };


                var res= await _authService.UpdateStaffProfile(id, up);
                if (res.IsSuccess) { 
                return RedirectToAction("Index");

                }

            }
        

            return View(staffFromReq);

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var staff = await _unitOfWork.StaffRepository.getAsync(ss => ss.Id == id);
            if (staff == null)
            {
                return NotFound();
            }
            return View("Delete", staff);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(string id)
        {
            Staff s = await _unitOfWork.StaffRepository.getAsync(s => s.Id == id);
            StaffSchedule ss = await _unitOfWork.StaffScheduleRepository.getAsync(s =>s.StaffId==id);

            if (s is null)
            {
                return NotFound();
            }
            //check if Staff is not assigned to any appointments in dates greater than today's date

            Appointment appointment = await _unitOfWork.AppointmentRepository.getAsync(a=>a.StaffId==id && a.AppointmentDateTime >=DateTime.Today);
            if (appointment != null) {

                TempData["Error"] = "Can not delete a Staff Member having upcoming appointments";
                return RedirectToAction("Index");

            }
            //mark the staff and staffschedule also deleted
            if (ss != null)
            {
                ss.IsDeleted = true;
                _unitOfWork.StaffScheduleRepository.Update(ss);

            }
            s.IsDeleted = true;
            _unitOfWork.StaffRepository.Update(s);

            await _unitOfWork.completeAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search() {

            return View();
        }

        [Authorize(Roles = "Patient")]

        public async Task<IActionResult>SearchStaff(string searchQuery)
        {

            if (string.IsNullOrEmpty(searchQuery)) { return NotFound(); }
            else { searchQuery = searchQuery.ToLower(); }
           
            var st=await _unitOfWork.StaffRepository.getAllAsync(s=> !s.IsDeleted && s.Position==Position.DOCTOR &&
           (s.Qualification.Contains(searchQuery)     || s.Department.Name.Contains(searchQuery)  || s.FullName.Contains(searchQuery)         )
           , new[] {"Department"}
           );
            if (st == null || !st.Any()) {

                return Json(new { success = false, message = "No matching doctors found." });

            }
            return PartialView("_SearchStaff", st);
        }




    }
}
