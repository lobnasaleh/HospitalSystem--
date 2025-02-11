
using HMS.Entites.Interfaces;
using HMS.Entities.Models;

namespace HMS.Entities.Interfaces
{
    public interface IDepartmentRepository:IBaseRepository<Department>
    {
        public List<Department> GetAllDepartments();

        public void AddDepartment(Department dept);

        public void Save();
    }
}
