using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models;

public partial class Doctor
{
    [Key]
    public int DoctorId { get; set; }
    [Required, MaxLength(100), MinLength(2)]
    public string Name { get; set; } = null!;
    [Required, MaxLength(100), MinLength(2)]
    public string? Surname { get; set; }
    [Required]
    public string? Specialty { get; set; }
    [Required, EmailAddress]
    public string? Email { get; set; }
    [Required, RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,}$")]
    public string? Password { get; set; }
    [Required, DefaultValue("Doctor"), AllowedValues("Doctor")]
    public string Role { get; set; } = null!;

    public virtual ICollection<AppointmentInfo> AppointmentInfos { get; set; } = new List<AppointmentInfo>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<MedicalRegister> MedicalRegisters { get; set; } = new List<MedicalRegister>();

    public virtual ICollection<WorkingDay> WorkingDays { get; set; } = new List<WorkingDay>();
}
