// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem2.Models;

public partial class Schedule
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Start time is required")]
    [DataType(DataType.Time)]
    public TimeOnly AvailableFrom { get; set; }

    [Required(ErrorMessage = "End time is required")]
    [DataType(DataType.Time)]

    public TimeOnly AvailableTo { get; set; }

    public bool IsDeleted { get; set; }


    public virtual ICollection<StaffSchedule> StaffSchedules { get; set; } = new List<StaffSchedule>();
}
