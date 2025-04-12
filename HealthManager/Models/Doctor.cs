using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models;

public partial class Doctor
{
    [Key]
    public int DoctorId { get; set; }

    public int LicenseNumber { get; set; }
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = null!;
    [Required]
    [MinLength(2)]
    public string Surname { get; set; } = null!;

    public int Specialty { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [MinLength(12)]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,}$")]
    public string Password { get; set; } = null!;

    public int? PhoneNumber { get; set; }

    public string? Role { get; set; } = "Doctor";

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<DoctorShift> DoctorShifts { get; set; } = new List<DoctorShift>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual Specialty SpecialtyNavigation { get; set; } = null!;

    public virtual ICollection<WorkingDay> WorkingDays { get; set; } = new List<WorkingDay>();
}
