using HMS.DataAccess.Data;
using HMS.Entities.Interfaces;
using HMS.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.DataAccess.Repository
{
    public class StaffRepository : BaseRepository<Staff>,IStaffRepository
    {
        HospitalContext context;
        public StaffRepository(HospitalContext _context):base(_context)
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
