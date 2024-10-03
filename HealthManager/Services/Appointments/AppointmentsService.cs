using HealthManager.Models;
using HealthManager.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using System.Numerics;
using System.Reflection;

namespace HealthManager.Services.Appointments
{
    public class AppointmentsService : IAppointments
    {
        private readonly HealthManagerContext _context;

        public AppointmentsService(HealthManagerContext context)
        {
            _context = context;
        }

        public async Task<MethodResponse> CreateAppointments()
        { 
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int currentYear = DateTime.Now.Year;

                var query = from Doctor in _context.Doctors
                            join AppointmentInfo in _context.AppointmentInfos
                            on Doctor.DoctorId equals AppointmentInfo.DoctorId
                            join workingDay in _context.WorkingDays
                            on Doctor.DoctorId equals workingDay.DoctorId
                            select new
                            {
                                Doctor.DoctorId,
                                DoctorName = Doctor.Name,
                                AppointmentStart = AppointmentInfo.WorkingHoursStart,
                                AppointmentEnd = AppointmentInfo.WorkingHoursEnd,
                                ConsultDuration = AppointmentInfo.ConsultationDuration,
                                workingDay
                            };

                var doctorsList = await query.ToListAsync();

                /*Type workingDayType = typeof(HealthManager.Models.WorkingDay);

                PropertyInfo[] properties = workingDayType.GetProperties();

                var dayProperties = properties.Where(prop =>
                    prop.Name == "Monday" || prop.Name == "Tuesday" || prop.Name == "Wednesday" ||
                    prop.Name == "Thursday" || prop.Name == "Friday" || prop.Name == "Saturday" ||
                    prop.Name == "Sunday"
                );*/

                List<string> daysOfWeek = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

                foreach (var doctor in doctorsList)
                {
                    int currentMonth = DateTime.Now.Month;
                    int daysInMonth = System.DateTime.DaysInMonth(currentYear, currentMonth);


                    var doctorWorkingDays = doctor.workingDay;
                    for (int i = 1; i <= daysInMonth; i++)
                    {
                        int currentDay = i;
                        foreach (var day in daysOfWeek)
                        {
                            DateTime auxiDay = new DateTime(currentYear, currentMonth, i);
                            string dayName = auxiDay.ToString("dddd", new CultureInfo("en-US"));
                            if (dayName.Equals(day))
                            {
                                var doctorConsultationStart = doctor.AppointmentStart;
                                var doctorConsultationEnd = doctor.AppointmentEnd;
                                var doctorConsultDuration = doctor.ConsultDuration;

                                var hourCount = auxiDay.Add(doctorConsultationStart.ToTimeSpan());
                                var limitHourAuxi = auxiDay.Add(doctorConsultationEnd.ToTimeSpan());

                                var durationAuxi = doctorConsultDuration.Minute + doctorConsultDuration.Hour * 60;

                                while (hourCount < limitHourAuxi)
                                {
                                    Appointment newAppointment = new Appointment
                                    {
                                        AppointmentId = new Guid(),
                                        AppointmentDate = new DateOnly(currentYear, currentMonth, currentDay),
                                        AppointmentHour = TimeOnly.FromDateTime(hourCount),
                                        Status = "Available",
                                        
                                        DoctorId = doctor.DoctorId,

                                    };
                                    await _context.Appointments.AddAsync(newAppointment);
                                    await _context.SaveChangesAsync();

                                    hourCount = hourCount.AddMinutes(durationAuxi);
                                }
                            }
                        }
                    }
                }
                await transaction.CommitAsync();
                return new MethodResponse { Success = true, Message = "Appointments created successfully" };
            }
            catch (Exception error)
            {

                await transaction.RollbackAsync();
                return new MethodResponse { Success = false, Message = error.ToString() };
            }
             
        }

        public async Task<MethodResponse> CheckForExistingRegisters()
        {
            int currentMonth = DateTime.Now.Month;
            var existingRegisters = await _context.Appointments.Where(a => a.AppointmentDate.Month == currentMonth).ToListAsync();
            if (existingRegisters.Count > 0)
            {
                return new MethodResponse{Success = true, Message="There are registers corresponding to the present month in the database"};
            }
            else
            {
                return new MethodResponse { Success = false, Message="There are no registers corresponding to actual month in the database. It is recommended to create new appointments in the database"};
            }
        }


    }
}
