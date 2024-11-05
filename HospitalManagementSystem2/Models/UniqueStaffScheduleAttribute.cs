using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem2.Models
{
    public class UniqueStaffScheduleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _context = (HospitalContext)validationContext.GetService(typeof(HospitalContext));
          
            var StaffSchedule = (StaffSchedule)validationContext.ObjectInstance;

            var ScheduleId= StaffSchedule.ScheduleId;

            int staffId = StaffSchedule.StaffId;

            if (_context.StaffSchedules.Any(ss=>ss.ScheduleId==ScheduleId && ss.StaffId==staffId))
            {
                return new ValidationResult("This Schedule is already assigned to this satff member.");
            }

            return ValidationResult.Success;
        }

    }
}
