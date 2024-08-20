using System;
using System.Collections.Generic;

namespace HealthManager.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Dni { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<MedicalRegister> MedicalRegisters { get; set; } = new List<MedicalRegister>();
}
