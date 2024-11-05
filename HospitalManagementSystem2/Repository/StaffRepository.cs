using HospitalManagementSystem2.Models;
using HospitalManagementSystem2.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem2.Repository
{
    public class StaffRepository : IStaffRepository
    {
        HospitalContext context;
        public StaffRepository(HospitalContext _context)
        {
            context = _context;
        }

        public List<Staff> GetStaff()
        {
            List< Staff> staff = context.Staff
                .Include(s=>s.User).ToList();
            return staff;
        }


       
    }
}
