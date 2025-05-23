﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models;

public partial class Appointment
{
    [Key]
    public Guid AppointmentId { get; set; }

    public int? PatientId { get; set; }
    public int DoctorId { get; set; }
    [Required]
    public DateOnly AppointmentDate { get; set; }
    [Required]
    public TimeOnly AppointmentHour { get; set; }
    [Required]
    public string? Status { get; set; }

    public bool? Attended { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual Patient? Patient { get; set; }
}
