using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem2.Controllers
{
    public class NavigateController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult PUser()
        {
            return View();
        }
        public IActionResult DUser()
        {
            return View();
        }
    }
}
