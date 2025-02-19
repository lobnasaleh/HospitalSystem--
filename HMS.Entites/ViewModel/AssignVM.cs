using HMS.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.ViewModel
{
    public class AssignVM
    {
        [Required]
        public int ScheduleId { get; set; }

        [Required]
        public string StaffId { get; set; }

        public IEnumerable<Schedule> Schedules { get; set; }=new List<Schedule>();

        public IEnumerable<Staff> Staff { get; set; }=new List<Staff>();
    }
}
