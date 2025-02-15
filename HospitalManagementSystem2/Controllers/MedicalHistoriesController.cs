using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc.Rendering;
using HMS.DataAccess.Data;
using HMS.Entities.Models;
using HMS.Entites.Interfaces;
using AutoMapper;
using HMS.Entites.ViewModel;

namespace HMS.web.Controllers
{
    public class MedicalHistoriesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MedicalHistoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet] //getting medical history of this dr
        public async Task<IActionResult> GetWrittenDoctorHistories()
        {
            // string docid = User.FindFirstValue(ClaimTypes.NameIdentifier); ma ansash azabtha

            var medicalhistorieswrittenbydoc = await unitOfWork.MedicalHistoriesRepository.getAllAsync(md => md.Appointment.StaffId == "1a2b3c4d-1234-5678-90ab-cdef12345678", new[] { "Appointment.Patient" });

            return View(medicalhistorieswrittenbydoc);
        }



        [HttpGet]
        public async Task<IActionResult> Add(int AppointmentId)
        {
            Appointment AP = await unitOfWork.AppointmentRepository.getAsync(ap => ap.Id == AppointmentId, false, new[] { "Patient" });
            if (AP == null) {
                return NotFound();
            }
            //map appointment to medicalhistoryvm
            MedicalHistoryVM mh = mapper.Map<MedicalHistoryVM>(AP);

            return View(mh);
        }

       // [HttpPost]

        /*  public async Task<IActionResult> Add(MedicalHistoryVM medicalhistoryfromreq)
          {
              if (ModelState.IsValid) {

                  //not having already a medical history for this appointment
                var md=  await unitOfWork.MedicalHistoriesRepository.getAsync(m=>m.AppointmentId==medicalhistoryfromreq.AppointmentId);
                  if (md != null) {

                      TempData["Error"] = "You have already written the Medical history for this appointment";
                      return RedirectToAction("GetWrittenDoctorHistories");

                  }

                  //map to medicalhistory
                  MedicalHistory mapped= mapper.Map<MedicalHistory>(medicalhistoryfromreq);
                  await unitOfWork.MedicalHistoriesRepository.AddAsync(mapped);
                  await unitOfWork.completeAsync();
               return RedirectToAction("AppointmentOfDoc", "Appointment");
              }


             // return View(mh);
          }
  */










    }


}