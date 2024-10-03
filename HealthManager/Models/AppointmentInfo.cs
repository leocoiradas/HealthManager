using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthManager.Models;

public partial class AppointmentInfo
{
    public int Id { get; set; }

    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }
    [Required]
    public TimeOnly WorkingHoursStart { get; set; }
    [Required]
    public TimeOnly WorkingHoursEnd { get; set; }
    [Required]
    public TimeOnly ConsultationDuration { get; set; }

    public int? WorkingDaysId { get; set; }
    public virtual Doctor Doctor { get; set; } = null!;

    public virtual WorkingDay? WorkingDays { get; set; }
}
