namespace HealthManager.Models.DTO
{
    public class PatientAppointmentsViewModel
    {
        public Guid AppointmentId { get; set; }

        public string DoctorName { get; set; }

        public string DoctorSpecialty { get; set; }
        
        public DateOnly AppointmentDate { get; set; }
        
        public TimeOnly AppointmentHour { get; set; }
    }
}
