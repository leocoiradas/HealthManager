using System;
using System.Collections.Generic;

namespace HealthManager.Models;

public partial class WorkingDay
{
    public int WorkingDaysId { get; set; }

    public int? DoctorId { get; set; }

    public bool? Monday { get; set; }

    public bool? Tuesday { get; set; }

    public bool? Wednesday { get; set; }

    public bool? Thursday { get; set; }

    public bool? Friday { get; set; }

    public bool? Saturday { get; set; }

    public bool? Sunday { get; set; }

    public virtual ICollection<AppointmentInfo> AppointmentInfos { get; set; } = new List<AppointmentInfo>();

    public virtual Doctor? Doctor { get; set; }
}
