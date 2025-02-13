﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using HMS.Entites.Enums;
using HMS.Entites.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Entities.Models;

public partial class Staff:ApplicationUser
{
   
   
    [Required(ErrorMessage = "Position is required.")]
    public Position Position { get; set; }
    [Required(ErrorMessage = "Qualification is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Qualification must be between 2 and 100 characters.")]

    public string Qualification { get; set; }

    [ForeignKey(nameof(Department))]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid department.")]
    [Required(ErrorMessage = "Department Name is required.")]
    public int DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<StaffSchedule>? StaffSchedules { get; set; } = new List<StaffSchedule>();

  /*  [ForeignKey(nameof(User))]
    [Required(ErrorMessage = "User's Name is Required")]
    public string UserId { get; set; }*/
    // public virtual ApplicationUser? User { get; set; }

}