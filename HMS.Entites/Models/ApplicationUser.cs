using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.Models
{
   public class ApplicationUser:IdentityUser
    {
        public string Address { get; set; }
        public string FullName{ get; set; }
        public bool IsDeleted { get; set; }

    }
}
