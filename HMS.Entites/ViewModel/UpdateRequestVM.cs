using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.ViewModel
{
    public class UpdateRequestVM //patient
    {
        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime DOB { get; set; }
        [Required]
        [Phone]
        [StringLength(15)]
        public string EmergencyContact { get; set; }
        [Required]
        [StringLength(100)]
        public string InsuranceProvider { get; set; }
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^\d{10,20}$", ErrorMessage = "Insurance number must be between 10 and 20 digits.")]
        public string InsuranceNumber { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Full Name can not exceed 50 characters!")]
        public string FullName { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Address can not exceed 200 characters!")]
        public string Address { get; set; }

    
    }
}
