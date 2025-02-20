using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.Contracts
{
    public class UpdateRequest //patient
    {
        public DateTime DOB { get; set; }
        public string EmergencyContact { get; set; }
        public string InsuranceProvider { get; set; }
        public string InsuranceNumber { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
    }
}
