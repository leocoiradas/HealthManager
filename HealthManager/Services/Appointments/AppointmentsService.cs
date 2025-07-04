using HealthManager.Models;
using HealthManager.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace HealthManager.Services.Appointments
{
    public class AppointmentsService : IAppointments
    {
        private readonly HealthManagerContext _context;

        public AppointmentsService(HealthManagerContext context)
        {
            _context = context;
        }

        public async Task<MethodResponse> CreateAppointmentsForAllDoctors()
        {
            var query = from Doctor in _context.Doctors
                        join DoctorShift in _context.DoctorShifts
                        on Doctor.DoctorId equals DoctorShift.DoctorId
                        join workingDay in _context.WorkingDays
                        on Doctor.DoctorId equals workingDay.DoctorId
                        select new
                        {
                            Doctor,
                            DoctorShift = DoctorShift,
                            WorkingDay = workingDay
                        };

            List<DoctorDTO> doctorsList = await query.Select(x => new DoctorDTO
            {
                Doctor = x.Doctor,
                DoctorShift = x.DoctorShift,
                WorkingDay = x.WorkingDay
            })
            .ToListAsync();

            await CreateDoctorAppointments(doctorsList, 1);

            return new MethodResponse { Success = true, Message = "Appointments created successfully" };

        }

        public async Task <MethodResponse> CreateDoctorAppointments(int doctorId)
        {
            var doctorProfile = await _context.Doctors
                .Where(x => x.DoctorId == doctorId)
                .Join(_context.DoctorShifts,
                    doc => doc.DoctorId,
                    shift => shift.DoctorId,
                    (doc, shift) => new
                    { doc, shift })
                .Join(_context.WorkingDays,
                    tmp => tmp.doc.DoctorId,
                    wd => wd.DoctorId,
                    (tmp, wd) => new 
                    {
                        Doctor = tmp.doc,
                        DoctorShift = tmp.shift,
                        WorkingDay = wd
                    })
                .FirstOrDefaultAsync();

            DoctorDTO newDoctorProfile = new DoctorDTO
            {
                Doctor = doctorProfile.Doctor,
                DoctorShift = doctorProfile.DoctorShift,
                WorkingDay = doctorProfile.WorkingDay
            };
            List<DoctorDTO> doctorList = [];
            doctorList.Add(newDoctorProfile);
            int currentDayNumber = DateTime.Now.Day;

            await CreateDoctorAppointments(doctorList, currentDayNumber);

            return new MethodResponse { Success = true, Message = "Appointments created successfully" };

        }

        public async Task<MethodResponse> CheckForExistingAppointments()
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

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;
                int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

                var query = from Doctor in _context.Doctors
                            join DoctorShift in _context.DoctorShifts
                            on Doctor.DoctorId equals DoctorShift.DoctorId
                            join workingDay in _context.WorkingDays
                            on Doctor.DoctorId equals workingDay.DoctorId
                            select new
                            {
                                Doctor.DoctorId,
                                DoctorName = Doctor.Name,
                                ShiftStart = DoctorShift.ShiftStart,
                                ShiftEnd = DoctorShift.ShiftEnd,
                                ConsultDuration = DoctorShift.ConsultDuration,
                                workingDay
                            };

                var doctorsList = await query.ToListAsync();

                

                foreach (var doctor in doctorsList)
                {
                    var doctorWorkingDays = doctor.workingDay;
                    for (int i = 1; i <= daysInMonth; i++)
                    {
                        int currentDay = i;
                        DateTime auxiDay = new DateTime(currentYear, currentMonth, currentDay);
                        string dayName = auxiDay.ToString("dddd", new CultureInfo("en-US"));
                        bool? isWorkingDay = (bool?)doctorWorkingDays.GetType().GetProperty(dayName)?.GetValue(doctorWorkingDays);
              
                        if (isWorkingDay.Equals(true))
                        {
                            var doctorConsultationStart = doctor.ShiftStart;
                            var doctorConsultationEnd = doctor.ShiftEnd;
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
                               

                                hourCount = hourCount.AddMinutes(durationAuxi);
                            }
                        }
                        
                    }
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                Console.WriteLine("--------------------------------");
                Console.WriteLine();
                Console.WriteLine("Appointments created successfully.");
                Console.WriteLine();
                Console.WriteLine("--------------------------------");
                return new MethodResponse { Success = true, Message = "Appointments created successfully" };
            }
            catch (Exception error)
            {

                await transaction.RollbackAsync();
                Console.WriteLine("--------------------------------");
                Console.WriteLine();
                Console.WriteLine("Error while creating the appointments.");
                Console.WriteLine();
                Console.WriteLine("--------------------------------");
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

        public async Task<MethodResponse> CheckAndCreateAppointments()
        {
            try
            {
                var existingRegisters = await CheckForExistingRegisters();

                if (existingRegisters.Success.Equals(true))
                {
                    return new MethodResponse { Success = false, Message = "There are already appointments for this month" };
                }
                else
                {
                    await CreateAppointments();
                    return new MethodResponse { Success = true, Message = "Appointments were successfully created." };
                }


            }
            catch (Exception error)
            {
                return new MethodResponse { Success = false, Message = error.ToString() };
                

            }

        }
    }
}
