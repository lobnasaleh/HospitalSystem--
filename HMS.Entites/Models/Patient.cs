﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using HMS.Entites.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Entities.Models;

public class Patient:ApplicationUser
{
   
    [Required]
    [DataType(DataType.Date)]
    [Column(TypeName = "date")]
    public DateTime DOB { get; set; }
    [Required]
    [Phone]
    [StringLength(15)]
    public string EmergencyContact { get; set; }
    [Required]
    [StringLength(100)]
    public string InsuranceProvider { get; set; }
    [Required]
    [StringLength(20)]
    [RegularExpression(@"^\d{10,20}$", ErrorMessage = "Insurance number must be between 10 and 20 digits.")]
    public string InsuranceNumber { get; set; }
    public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();


   /* [ForeignKey(nameof(User))]
    public string UserId { get; set; }*/
    // public virtual ApplicationUser? User { get; set; }

}