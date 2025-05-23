using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class MedicalRecordViewModel
    {
        public Guid AppointmentId { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; }
        public int PatientId { get; set; }

        public string PatientName { get; set; }
        [Required]
        public string Diagnosis { get; set; } = null!;
        [Required]
        public string Observations { get; set; } = null!;
        [Required]
        public string Treatment { get; set; } = null!;

        public DateTime RecordDate { get; set ; }
    }
}
