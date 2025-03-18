using System;
using System.Collections.Generic;

namespace HealthManager.Models;

public partial class MedicalRecord
{
    public Guid Id { get; set; }

    public Guid? AppointmentId { get; set; }

    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public DateTime? Date { get; set; }

    public string Diagnosis { get; set; } = null!;

    public string Observations { get; set; } = null!;

    public string Treatment { get; set; } = null!;

    public virtual Appointment? Appointment { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
