using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.ViewModel
{
    public class ScheduleVM
    {
        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [DataType(DataType.Time)]
        public TimeOnly? AvailableFrom { get; set; }

        [Required(ErrorMessage = "End time is required")]
        [DataType(DataType.Time)]
        public TimeOnly? AvailableTo { get; set; }
    }
}
