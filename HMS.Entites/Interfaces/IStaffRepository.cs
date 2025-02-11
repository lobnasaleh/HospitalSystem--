

using HMS.Entites.Interfaces;
using HMS.Entities.Models;

namespace HMS.Entities.Interfaces
{
    public interface IStaffRepository:IBaseRepository<Staff>
    {
        List<Staff> GetStaff();
    }
}
