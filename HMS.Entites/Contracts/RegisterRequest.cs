using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.Contracts
{
   public class RegisterRequest
    {
        //patient normal register

      
        public DateTime DOB { get; set; }
        public string EmergencyContact { get; set; }
        public string InsuranceProvider { get; set; }
        public string InsuranceNumber { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PasswordHash { get; set; }
   
    }
}
