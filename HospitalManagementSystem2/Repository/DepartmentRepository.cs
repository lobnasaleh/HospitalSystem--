using HospitalManagementSystem2.Models;
using HospitalManagementSystem2.Repository.Interfaces;

namespace HospitalManagementSystem2.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        HospitalContext Context;

        public DepartmentRepository(HospitalContext context)
        {
            Context = context;
        }
        public void AddDepartment(Department dept)
        {
            Context.Departments.Add(dept);
        }

        public List<Department> GetAllDepartments()
        {
            List<Department> departments = Context.Departments.ToList();
            return departments;
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
