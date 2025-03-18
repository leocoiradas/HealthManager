using System;
using System.Collections.Generic;

namespace HealthManager.Models;

public partial class DoctorShift
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public TimeOnly ShiftStart { get; set; }

    public TimeOnly ShiftEnd { get; set; }

    public TimeOnly ConsultDuration { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;
}
