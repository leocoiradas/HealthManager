using System;
using System.Collections.Generic;

namespace HealthManager.Models;

public partial class Appointment
{
    public Guid AppointmentId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly AppointmentHour { get; set; }

    public string Status { get; set; } = null!;

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
