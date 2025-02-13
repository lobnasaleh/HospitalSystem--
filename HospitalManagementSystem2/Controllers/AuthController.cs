using AutoMapper;
using HMS.DataAccess.Repository;
using HMS.Entites.Contracts;
using HMS.Entites.Interfaces;
using HMS.Entites.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HMS.web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
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
    }
}
