using System.ComponentModel.DataAnnotations;

namespace HealthManager.Models.DTO
{
    public class AppointmentDataPDFDTO
    {
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentHour { get; set; }
        public string Specialty { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }

    }
}
