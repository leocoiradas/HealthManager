using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class AppointmentViewModel
    {
        public Guid AppointmentId { get; set; }
        [Required]
        public DateOnly AppointmentDate { get; set; }
        [Required]
        public TimeOnly AppointmentHour { get; set; }
        [Required]
        public string Specialty { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int DoctorId { get; set; }
    }
}
