using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.ViewModel
{
    public class AppointmentOfDoctorVM
    {
        public string PatientId { get; set; }

        public int DepartmentId { get; set; }

        public DateTime AppointmentDateTime { get; set; }
    }
}
