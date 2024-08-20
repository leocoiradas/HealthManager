using System;
using System.Collections.Generic;

namespace HealthManager.Models;

public partial class AppointmentInfo
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public TimeOnly WorkingHoursStart { get; set; }

    public TimeOnly WorkingHoursEnd { get; set; }

    public TimeOnly ConsultationDuration { get; set; }

    public int? WorkingDaysId { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual WorkingDay? WorkingDays { get; set; }
}
