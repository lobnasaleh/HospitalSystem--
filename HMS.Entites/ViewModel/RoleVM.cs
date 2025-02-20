using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.ViewModel
{
    public class RoleVM
    {
        [Required]
        public string Name { get; set; }
    }
}
