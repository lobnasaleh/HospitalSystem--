using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem2.Models
{
    public class UniqueDepartmentNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _context = (HospitalContext)validationContext.GetService(typeof(HospitalContext));
            var departmentName = value as string;

            if (_context.Departments.Any(d => d.Name == departmentName))
            {
                return new ValidationResult("Department name already exists.");
            }

            return ValidationResult.Success;
        }

    }
}
