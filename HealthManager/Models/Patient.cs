using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models;

public partial class Patient
{
    public int PatientId { get; set; }
    [Required]
    [MaxLength(100)]
    [MinLength(6)]
    public int? Dni { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Surname { get; set; } = null!;
    [Required]
    public DateOnly BirthDate { get; set; }
    [Required]
    [MaxLength(1)]
    public string? Gender { get; set; }
    [Required]
    [MaxLength(1)]
    public string? Sex { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [MinLength(12)]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,}$")]
    public string Password { get; set; } = null!;
    [Required]
    public int? PhoneNumber { get; set; }

    public string? Role { get; set; } = "Patient";

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual ICollection<MedicalRegister> MedicalRegisters { get; set; } = new List<MedicalRegister>();
}
