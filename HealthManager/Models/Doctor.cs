using System;
using System.Collections.Generic;

namespace HealthManager.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string Name { get; set; } = null!;

    public string? Surname { get; set; }

    public string? Spectialty { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<AppointmentInfo> AppointmentInfos { get; set; } = new List<AppointmentInfo>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<MedicalRegister> MedicalRegisters { get; set; } = new List<MedicalRegister>();

    public virtual ICollection<WorkingDay> WorkingDays { get; set; } = new List<WorkingDay>();
}
