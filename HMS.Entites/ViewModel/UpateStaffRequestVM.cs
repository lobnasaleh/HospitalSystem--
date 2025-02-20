using HMS.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.ViewModel
{
    public class UpateStaffRequestVM
    {
        [Required(ErrorMessage = "Position is required.")]
        public int Position { get; set; }
        [Required(ErrorMessage = "Qualification is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Qualification must be between 2 and 100 characters.")]

        public string Qualification { get; set; }

        [ForeignKey(nameof(Department))]
        // [Range(1, int.MaxValue, ErrorMessage = "Please select a valid department.")]
        [Required(ErrorMessage = "Department Name is required.")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Full Name can not exceed 50 characters!")]
        public string FullName { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Address can not exceed 200 characters!")]
        public string Address { get; set; }

    }
}
