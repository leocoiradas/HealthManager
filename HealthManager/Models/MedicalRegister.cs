using System;
using System.Collections.Generic;

namespace HealthManager.Models;

public partial class MedicalRegister
{
    public Guid RegisterId { get; set; }

    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public DateTime Date { get; set; }

    public string Title { get; set; } = null!;

    public string Resume { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
