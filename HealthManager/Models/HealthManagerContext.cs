using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HealthManager.Models;

public partial class HealthManagerContext : DbContext
{
    public HealthManagerContext()
    {
    }

    public HealthManagerContext(DbContextOptions<HealthManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentInfo> AppointmentInfos { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<MedicalRegister> MedicalRegisters { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<WorkingDay> WorkingDays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC2E6F11257");

            entity.Property(e => e.AppointmentId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AppointmentHour).HasPrecision(0);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Docto__5070F446");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Appointme__Patie__4F7CD00D");
        });

        modelBuilder.Entity<AppointmentInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Appointm__3214EC07543850FF");

            entity.ToTable("AppointmentInfo");

            entity.Property(e => e.ConsultationDuration).HasPrecision(0);
            entity.Property(e => e.WorkingHoursEnd).HasPrecision(0);
            entity.Property(e => e.WorkingHoursStart).HasPrecision(0);

            entity.HasOne(d => d.Doctor).WithMany(p => p.AppointmentInfos)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Docto__5FB337D6");

            entity.HasOne(d => d.WorkingDays).WithMany(p => p.AppointmentInfos)
                .HasForeignKey(d => d.WorkingDaysId)
                .HasConstraintName("FK__Appointme__Worki__60A75C0F");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EBFADCBDF5D");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Doctor");
            entity.Property(e => e.Specialty)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MedicalRegister>(entity =>
        {
            entity.HasKey(e => e.RegisterId).HasName("PK__MedicalR__B91FAB794189BCFA");

            entity.Property(e => e.RegisterId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Resume).HasColumnType("text");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Doctor).WithMany(p => p.MedicalRegisters)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MedicalRe__Docto__5441852A");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalRegisters)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MedicalRe__Patie__5535A963");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC3666E7CDD06");

            entity.Property(e => e.Dni).HasColumnName("DNI");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Patient");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WorkingDay>(entity =>
        {
            entity.HasKey(e => e.WorkingDaysId).HasName("PK__WorkingD__1913AC2EA00AECF7");

            entity.Property(e => e.WorkingDaysId).HasColumnName("WorkingDaysID");

            entity.HasOne(d => d.Doctor).WithMany(p => p.WorkingDays)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__WorkingDa__Docto__5CD6CB2B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
