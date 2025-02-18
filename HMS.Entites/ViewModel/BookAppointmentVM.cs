using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.ViewModel
{
    public class BookAppointmentVM
    {
        
        public  string StaffId {  get; set; }
        public int  DepartmentId { get; set; }
        public string  PatientId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}
