using HospitalManagementSystem2.Models;

namespace HospitalManagementSystem2.Repository.Interfaces
{
    public interface IDepartmentRepository
    {
        public List<Department> GetAllDepartments();

        public void AddDepartment(Department dept);

        public void Save();
    }
}
