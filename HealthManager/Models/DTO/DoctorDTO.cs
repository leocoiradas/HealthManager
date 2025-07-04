namespace HealthManager.Models.DTO
{
    public class DoctorDTO
    {
        public Doctor Doctor { get; set; } = null!;
        public WorkingDay WorkingDay { get; set; } = null!;
        public DoctorShift DoctorShift { get; set; } = null!;
    }
}
