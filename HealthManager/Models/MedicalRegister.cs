using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models;

public partial class MedicalRegister
{
    public Guid RegisterId { get; set; }

    public int DoctorId { get; set; }

    public int PatientId { get; set; }
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Resume { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
