using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models;

public partial class WorkingDay
{
    public int WorkingDaysId { get; set; }

    public int? DoctorId { get; set; }

    [Required]
    public bool? Monday { get; set; }

    [Required]
    public bool? Tuesday { get; set; }

    [Required]
    public bool? Wednesday { get; set; }

    [Required]
    public bool? Thursday { get; set; }

    [Required]
    public bool? Friday { get; set; }

    [Required]
    public bool? Saturday { get; set; }

    [Required]
    public bool? Sunday { get; set; }

    public virtual ICollection<AppointmentInfo> AppointmentInfos { get; set; } = new List<AppointmentInfo>();

    public virtual Doctor? Doctor { get; set; }
}
