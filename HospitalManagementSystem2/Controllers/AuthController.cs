using AutoMapper;
using HMS.DataAccess.Repository;
using HMS.DataAccess.Services.AuthService;
using HMS.Entites.Contracts;
using HMS.Entites.Interfaces;
using HMS.Entites.ViewModel;
using HMS.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace HMS.web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthController(IAuthService authService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.authService = authService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
           var res= await authService.GetRoles();

            return View(res);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> AddRole(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
               string rolename = roleVM.Name;
               var res= await authService.AddRole(rolename);
                if (res.IsSuccess)
                {
                    return RedirectToAction("GetRoles");
                }
            }
            return View(roleVM);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View("RegisterPatient");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestVM registerRequestVM)
        {
            if (!ModelState.IsValid)
            {
                return View("RegisterPatient", registerRequestVM);
            }

            var req = mapper.Map<RegisterRequest>(registerRequestVM);

            await authService.RegisterPatient(req);//need to handle returned token

            return RedirectToAction("Index", "Home");
        }
        //update 
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            //getting the logged in user 
          //  string loggedinuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
          
            Patient p = await unitOfWork.patientRepository.getAsync(s => !s.IsDeleted && s.Id == "7E596CCF-CCA2-480C-B830-BBB6513D7309", false);
            if (p == null)
            {
                return NotFound();
            }

            UpdateRequestVM pamp = new UpdateRequestVM()
            {
                FullName = p.FullName,
                DOB=p.DOB,
                InsuranceNumber = p.InsuranceNumber,
                InsuranceProvider=p.InsuranceProvider,
                Address = p.Address,
                EmergencyContact = p.EmergencyContact
            };//from staff to staffmp

            return View(pamp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateRequestVM pFromReq)
        {
            
            if (ModelState.IsValid)
            {

                //var sttt = mapper.Map<UpateStaffRequest>(staffFromReq);
                UpdateRequest up = new UpdateRequest()
                {
                   Address = pFromReq.Address,
                   EmergencyContact=pFromReq.EmergencyContact,  
                   InsuranceProvider = pFromReq.InsuranceProvider,
                   InsuranceNumber=pFromReq.InsuranceNumber,
                   DOB = pFromReq.DOB,
                   FullName = pFromReq.FullName 
                };

                // var loggedinuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                

                var res = await authService.UpdateProfile("7E596CCF-CCA2-480C-B830-BBB6513D7309", up);
                if (res.IsSuccess)
                {
                    return RedirectToAction("Index","Home");

                }
                else
                {
                    ViewData["Error"]=res.ToString();
                    return View(pFromReq);
                }

            }

            return View(pFromReq);

        }

        //login

        //getpatientmedicalhistories
    }
}
