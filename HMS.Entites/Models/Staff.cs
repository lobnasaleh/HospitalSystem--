﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using HMS.Entites.Enums;
using HMS.Entites.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Entities.Models;

public partial class Staff
{
    public int Id { get; set; }

    public Position Position { get; set; }

    public string Qualification { get; set; }

    [ForeignKey(nameof(User))]
    public string UserId { get; set; }
    [ForeignKey(nameof(Department))]
    public int DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<StaffSchedule>? StaffSchedules { get; set; } = new List<StaffSchedule>();

    
    public virtual ApplicationUser? User { get; set; }

}