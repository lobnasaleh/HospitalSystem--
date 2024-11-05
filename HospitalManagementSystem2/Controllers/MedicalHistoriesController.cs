using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagementSystem2.Controllers
{
    public class MedicalHistoriesController : Controller
    {
        private readonly HospitalContext _context;

        public MedicalHistoriesController(HospitalContext context)
        {
            _context = context;
        }

        // GET: MedicalHistories (for users without specifying patient ID)
        public async Task<IActionResult> Index()
        {
            var hospitalContext = _context.MedicalHistories.Include(m => m.Patient).Include(m => m.Staff);
            return View(await hospitalContext.ToListAsync());
        }

        // GET: MedicalHistories/PatientHistory/5 (for patients with specified patient ID)
        public async Task<IActionResult> PatientHistory(int? patientId)
        {
            if (patientId == null)
            {
                return NotFound();
            }

            var medicalHistories = await _context.MedicalHistories
                .Include(m => m.Patient)
                .Include(m => m.Staff)
                .Where(m => m.PatientId == patientId)
                .ToListAsync();

            if (medicalHistories == null || !medicalHistories.Any())
            {
                return NotFound();
            }

            return View("PatientHistory", medicalHistories);
        }

        // GET: MedicalHistories/CreateOrUpdate (for showing the create/edit form)
        public IActionResult CreateOrUpdate(int? id)
        {
            // NEW: Populate ViewBag for dropdowns
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name"); // NEW
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Name"); // NEW

            if (id == null)
            {
                // Creating a new medical history
                return View(new MedicalHistory());
            }
            else
            {
                // Editing an existing medical history
                var medicalHistory = _context.MedicalHistories.Find(id);
                if (medicalHistory == null)
                {
                    return NotFound();
                }
                return View(medicalHistory);
            }
        }

        // POST: MedicalHistories/CreateOrUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdate([Bind("Id,PatientId,StaffId,Diagnosis,TreatmentPlan,Prescription,VisitedDate")] MedicalHistory medicalHistory)
        {
            if (ModelState.IsValid)
            {
                if (medicalHistory.Id == 0)
                {
                    _context.Add(medicalHistory);
                }
                else
                {
                    _context.Update(medicalHistory);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // NEW: Populate ViewBag for dropdowns in case of model validation failure
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", medicalHistory.PatientId); // NEW
            ViewData["StaffId"] = new SelectList(_context.Staff, "Id", "Name", medicalHistory.StaffId); // NEW
            return View(medicalHistory);
        }

        private bool MedicalHistoryExists(int id)
        {
            return _context.MedicalHistories.Any(e => e.Id == id);
   }




        // GET: MedicalHistories/Delete/
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalHistory = await _context.MedicalHistories
                .Include(m => m.Patient)
                .Include(m => m.Staff)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicalHistory == null)
            {
                return NotFound();
            }

            return View(medicalHistory);
        }

        // POST: MedicalHistories/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalHistory = await _context.MedicalHistories.FindAsync(id);
            if (medicalHistory != null)
            {
                _context.MedicalHistories.Remove(medicalHistory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }








    }

















}
