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
using Microsoft.CodeAnalysis;

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
        public async Task<IActionResult> GetMedicalHistoriesOfPatient()//a3melha endpoint
        {
         //  string loggedinuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
         var res= await unitOfWork.MedicalHistoriesRepository.getAllAsync(m => m.Appointment.Patient.Id == "7E596CCF-CCA2-480C-B830-BBB6513D7309", new[] { "Appointment.Patient" });
        
         return View("PatientHistory",res);
        }


        [HttpGet]
        public async Task<IActionResult> Add(int AppointmentId)
        {
            Appointment AP = await unitOfWork.AppointmentRepository.getAsync(ap => ap.Id == AppointmentId, false, new[] { "Patient" });
            if (AP == null) {
                return NotFound();
            }
            //map appointment to medicalhistoryvm
            MedicalHistoryVM mh = new MedicalHistoryVM()
            {
                AppointmentDateTime = AP.AppointmentDateTime,
                FullName = AP.Patient.FullName,
                AppointmentId=AppointmentId
            };


    

            return View(mh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Add(MedicalHistoryVM medicalhistoryfromreq)
        {
            if (ModelState.IsValid)
            {

                //not having already a medical history for this appointment
                var md = await unitOfWork.MedicalHistoriesRepository.getAsync(m => m.AppointmentId == medicalhistoryfromreq.AppointmentId);
                if (md != null)
                {

                    TempData["Error"] = "You have already written the Medical history for this appointment";
                    return RedirectToAction("GetWrittenDoctorHistories");

                }

                //map to medicalhistory
                MedicalHistory mapped = new MedicalHistory()
                {
                    AppointmentId=medicalhistoryfromreq.AppointmentId,
                    Diagnosis=medicalhistoryfromreq.Diagnosis,
                    Prescription=medicalhistoryfromreq.Prescription,
                    TreatmentPlan= medicalhistoryfromreq.TreatmentPlan
                };


                //MedicalHistory mapped = mapper.Map<MedicalHistory>(medicalhistoryfromreq);
                await unitOfWork.MedicalHistoriesRepository.AddAsync(mapped);
                await unitOfWork.completeAsync();
                return RedirectToAction("AppointmentOfDoc", "Appointment");
            }


            return View(medicalhistoryfromreq);//3yza ata2ked mazboota wala eh
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)//medicalHistoryId
        {
            MedicalHistory mh = await unitOfWork.MedicalHistoriesRepository.getAsync(m=>m.Id==id, false, new[] { "Appointment.Patient" });
            if (mh == null)
            {
                return NotFound();
            }
            //map MedicalHistory to medicalhistoryvm
            MedicalHistoryVM med = new MedicalHistoryVM()
            {
                AppointmentId=mh.AppointmentId,
                FullName=mh.Appointment.Patient.FullName,
                AppointmentDateTime=mh.Appointment.AppointmentDateTime,
                Diagnosis=mh.Diagnosis,
                Prescription=mh.Prescription,
                TreatmentPlan = mh.TreatmentPlan
            };
            return View(med);
        }

         [HttpPost]
         [ValidateAntiForgeryToken] 

        public async Task<IActionResult> Update(MedicalHistoryVM medicalhistoryfromreq)
        {
            if (ModelState.IsValid)
            {

                //not having already a medical history for this appointment
                var md = await unitOfWork.MedicalHistoriesRepository.getAsync(m => m.AppointmentId == medicalhistoryfromreq.AppointmentId);
                if (md == null)
                {

                    TempData["Error"] = "No medical history exists for this appointment.";
                    return RedirectToAction("GetWrittenDoctorHistories");
                }

                md.Diagnosis = medicalhistoryfromreq.Diagnosis;
                md.Prescription = medicalhistoryfromreq.Prescription;
                md.TreatmentPlan = medicalhistoryfromreq.TreatmentPlan;

                unitOfWork.MedicalHistoriesRepository.Update(md);
                await unitOfWork.completeAsync();
                return RedirectToAction("GetWrittenDoctorHistories");
            }


            return View(medicalhistoryfromreq);//3yza ata2ked mazboota wala eh
        }
        //getMedicalHistoroesByPatient

    }


}