using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.ViewModel
{
    public class DepartmentViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Department Name can not exceed 50 characters!")]
        public string Name { get; set; }
    }
}
