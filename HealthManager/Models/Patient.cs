using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models;

public partial class Patient
{
    public int PatientId { get; set; }
    [Required]
    [MaxLength(100)]
    [MinLength(1)]
    public string Name { get; set; } = null!;
    [MaxLength(100)]
    [MinLength(1)]
    public string Surname { get; set; } = null!;
    [Required]
    public DateOnly Birthdate { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required, MinLength(12), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,}$")]
    public string Password { get; set; } = null!;
    [Required]
    public int Dni { get; set; }
    [DefaultValue("Patient"), AllowedValues("Patient")]
    public string Role { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<MedicalRegister> MedicalRegisters { get; set; } = new List<MedicalRegister>();
}
