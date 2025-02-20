using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.Contracts
{
    public class UpateStaffRequest
    {
        public int Position { get; set; }
        public string Qualification { get; set; }
        public int DepartmentId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
    }
}
