using HMS.Entites.Enums;
using HMS.Entities.Models;
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
   public class RegisterStaffRequest
    {
      
        public int Position { get; set; }
        public string Qualification { get; set; }
        public int DepartmentId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PasswordHash { get; set; }
   
    }
}
