using HMS.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.ViewModel
{
    public class MedicalHistoryVM
    {

        public int AppointmentId { get; set; }
        public string FullName { get; set; }//PatientName //readonly

        public DateTime AppointmentDateTime { get; set; } //readonly


        [MaxLength(500, ErrorMessage = "Diagnosis can not exceed 500 characters")]
        [Required(ErrorMessage = "Diagnosis is required.")]
        public string Diagnosis { get; set; }
        [MaxLength(1000, ErrorMessage = "Treatment plan cannot exceed 1000 characters.")]
        [Required(ErrorMessage = "Treatment plan is required.")]
        public string TreatmentPlan { get; set; }
        [MaxLength(500, ErrorMessage = "Prescription can not exceed 500 characters")]
        [Required(ErrorMessage = "Prescription is required.")]

        public string Prescription { get; set; }

    }
}
