using HealthManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthManager.Services.Appointments
{
    public class AppointmentsService : IAppointments
    {
        private readonly HealthManagerContext _context;

        public AppointmentsService(HealthManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CreateAppointments()
        {
            int currentYear = DateTime.Now.Year;
            List<Doctor> doctorsCollection = _context.Doctors
                .Include(d => d.AppointmentInfos)
                .Include(d => d.WorkingDays)
                .ToList();
            foreach(Doctor doctor in doctorsCollection)
            {
                int currentMonth = DateTime.Now.Month;
                int daysInMonth = System.DateTime.DaysInMonth(currentYear, currentMonth);

                List<WorkingDay> doctorWorkingDays = doctor.WorkingDays.ToList();
                for (int i=1; i<=daysInMonth; i++)
                {
                    int currentDay = i;

                }
            }
        }

        public async Task<Appointment> GetAvailableAppointments()
        {
            int month = 1;
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            List<Appointment> appointments = _context.Appointments
                .Where(x => x.AppointmentDate.Month == currentMonth && x.AppointmentDate.Year == currentYear)
                .ToList();

            throw new NotImplementedException();
        }


    }
}
