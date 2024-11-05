using HospitalManagementSystem2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.CodeDom;

namespace HospitalManagementSystem2.Controllers
{
    public class PatientController : Controller
    {
        HospitalContext context;
        public PatientController(HospitalContext hospitalContext)
        {
            context=hospitalContext;
        }
        [HttpGet]
        public IActionResult GetAllPatients()
        {           
            List<Patient> patients = context.Patients.Include(p => p.User).Where(e=>e.User.IsDeleted==false).ToList();
            
            return View("GetAllPatients", patients);
        }
        [HttpGet]
        public IActionResult GetPatientById(int id)
        {
            Patient patient = context.Patients.Include(p => p.User).FirstOrDefault(e=>e.Id==id);
            if (patient == null)
            {
                TempData["ErrorMessage"] = "Patient not found!";
                return RedirectToAction("GetAllPatients");
            }
            return View("GetPatientById", patient);
        }
        [HttpGet]
        public IActionResult Add(User userFromReq)
        {
            ViewData["userId"] = userFromReq.Id;
            ViewData["usersList"] = context.Users.ToList();
            return View("AddPatient");
        }
        [HttpPost]
        public IActionResult AddPatient(Patient patientFromReq) {
            
            if (patientFromReq.InsuranceProvider != null
                &&patientFromReq.InsuranceNumber!=null
                &&patientFromReq.Dob!=null
                &&patientFromReq.Address!=null
                &&patientFromReq.EmergencyContact!=null
                ) {
                try
                {
                    
                        context.Patients.Add(patientFromReq);
                        context.SaveChanges();
                    

                    return RedirectToAction("GetAllPatients");
                }
                catch (Exception ex)
                {
                    return Content("there  are error in db");
                }
            }
            
            ViewData["usersList"] = context.Users.ToList();
            return View("AddPatient", patientFromReq);

        }
        [HttpGet]
        public IActionResult EditPatient(User userFromReq)
        {
            Patient patient = context.Patients.FirstOrDefault(e => e.UserId == userFromReq.Id);
            if (patient == null)
            {
                return RedirectToAction("GetAllPatients");
            }
            ViewData["userId"] = userFromReq.Id;
            ViewData["usersList"] = context.Users.ToList();
            return View("EditPatient", patient);

        }
        [HttpPost]
        public IActionResult SaveEditPatient(Patient patientFromReq )
        {
            if (patientFromReq.InsuranceProvider != null
                && patientFromReq.InsuranceNumber != null
                && patientFromReq.Dob != null
                && patientFromReq.Address != null
                && patientFromReq.EmergencyContact != null
           
                )
            {
                User user = context.Users.FirstOrDefault(e=>e.Id == patientFromReq.UserId);
                Patient patientFromDb = context.Patients.FirstOrDefault(e=>e.UserId== user.Id);
                if (patientFromDb != null)
                {
                    try
                    {
                        patientFromDb.Address = patientFromReq.Address;
                        patientFromDb.InsuranceNumber = patientFromReq.InsuranceNumber;
                        patientFromDb.InsuranceProvider = patientFromReq.InsuranceProvider;
                        patientFromDb.EmergencyContact = patientFromReq.EmergencyContact;
                        patientFromDb.Dob = patientFromReq.Dob;
                        patientFromDb.UserId = patientFromReq.UserId;                       
                        context.SaveChanges();
                        return RedirectToAction("GetAllPatients");
                    }
                    catch (Exception ex) {
                        return Content("there are problem in db");
                    }
                   
                }
                return NotFound("not found");
            }
            
            ViewData["usersList"] = context.Users.ToList();
            return View("EditPatient", patientFromReq);


        }

        [HttpGet]
        public IActionResult DeletePatient(int id)
        {

            Patient patient = context.Patients.FirstOrDefault(e => e.Id == id);
            if (patient == null)
            {
                return RedirectToAction("GetAllPatients");
            }

            return View("DeletePatient", patient);



        }
        [HttpPost]
        public IActionResult SaveDeletePatient(int id)
        {
            Patient patient = context.Patients.FirstOrDefault(e => e.Id == id);
           
            if (patient != null)
            {
                User userFromDb = context.Users.FirstOrDefault(e => e.Id == patient.UserId);
                try
                {

                    userFromDb.IsDeleted = true;
                    userFromDb.Name = userFromDb.Name;
                    userFromDb.Email = userFromDb.Email;
                    userFromDb.PhoneNumber = userFromDb.PhoneNumber;
                    userFromDb.RoleId = userFromDb.RoleId;
                    userFromDb.Password = userFromDb.Password;
                    context.SaveChanges();


                    return RedirectToAction("GetAllPatients");
                }
                catch (Exception ex)
                {
                    return Content("there  are error in db");
                }


            }
            return NotFound();

       }
        [HttpGet]
        public IActionResult AddU()
        {
            ViewData["RolesList"] = context.Roles.ToList();
            return View("AddUser");
        }
     
        [HttpPost]
        public IActionResult AddUser(User userFromReq)
        {

            if (userFromReq.Name != null
                && userFromReq.Email != null
                && userFromReq.Password != null
                && userFromReq.PhoneNumber != null
                )
            {
                try
                {

                    context.Users.Add(userFromReq);
                    context.SaveChanges();
                    return RedirectToAction("Add", userFromReq);
                }
                catch (Exception ex)
                {
                    return Content("there  are error in db");
                }
            }

            ViewData["RolesList"] = context.Roles.ToList();
            return View("AddUser", userFromReq);

        }

        [HttpGet]
        public IActionResult EditU(int id)
        {
            User user = context.Users.FirstOrDefault(e => e.Id == id);
            if (user == null)
            {
                return RedirectToAction("GetAllPatients");
            }
            ViewData["RolesList"] = context.Roles.ToList();
            return View("EditUser", user);

        }
        [HttpPost]
        public IActionResult SaveEditUser(User userFromReq, int id)
        {
            if (userFromReq.Name != null
                && userFromReq.Email != null
                && userFromReq.Password != null
                && userFromReq.PhoneNumber != null
                

                )
            {
                User userFromDb = context.Users.FirstOrDefault(e => e.Id == id);
                if (userFromDb != null)
                {
                    try
                    {
                        userFromDb.Name = userFromReq.Name;
                        userFromDb.Email = userFromReq.Email;
                        userFromDb.Password = userFromReq.Password;
                        userFromDb.PhoneNumber = userFromReq.PhoneNumber;                      
                        userFromDb.RoleId = userFromReq.RoleId;
                        context.SaveChanges();
                        return RedirectToAction("EditPatient", userFromDb);
                    }
                    catch (Exception ex)
                    {
                        return Content("there are problem in db");
                    }

                }
                return NotFound(id);
            }

            ViewData["RolesList"] = context.Roles.ToList();
            return View("EditUser", userFromReq);


        }
    }
}
