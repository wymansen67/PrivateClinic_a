using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaPrivateClinic.Models;

public partial class ClinicDbContext : DbContext
{
    public ClinicDbContext()
    {
    }

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<Equipment> Equipments { get; set; }

    public virtual DbSet<MedicalCheckupPlan> MedicalCheckupPlans { get; set; }

    public virtual DbSet<MedicalCheckupType> MedicalCheckupTypes { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<OfficeType> OfficeTypes { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Purpose> Purposes { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Specialist> Specialists { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<User> Users { get; set; }
    //172.24.250.145
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=192.168.1.82;Database=private_clinic;Username=postgres;Password=728281938293DA;Pooling=true;MinPoolSize=1;MaxPoolSize=20;Timeout=30;CommandTimeout=60");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentNumber).HasName("appointments_pkey");

            entity.ToTable("appointments");

            entity.Property(e => e.AppointmentNumber)
                .HasDefaultValueSql("nextval('appointments_id_seq'::regclass)")
                .HasColumnName("appointment_number");
            entity.Property(e => e.Commentaries).HasColumnName("commentaries");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.IsPlanned).HasColumnName("isPlanned");
            entity.Property(e => e.Office).HasColumnName("office");
            entity.Property(e => e.Patient).HasColumnName("patient");
            entity.Property(e => e.Purpose).HasColumnName("purpose");
            entity.Property(e => e.Specialist).HasColumnName("specialist");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("money")
                .HasColumnName("total_price");

            entity.HasOne(d => d.OfficeNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Office)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_office");

            entity.HasOne(d => d.PatientNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Patient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_patient");

            entity.HasOne(d => d.PurposeNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Purpose)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_purpose");

            entity.HasOne(d => d.SpecialistNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Specialist)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_specialist");

            entity.HasMany(d => d.Diagnoses).WithMany(p => p.Appointments)
                .UsingEntity<Dictionary<string, object>>(
                    "AppointmentDiagnosis",
                    r => r.HasOne<Diagnosis>().WithMany()
                        .HasForeignKey("Diagnosis")
                        .HasConstraintName("fkey_diagnosis"),
                    l => l.HasOne<Appointment>().WithMany()
                        .HasForeignKey("Appointment")
                        .HasConstraintName("fkey_appointment"),
                    j =>
                    {
                        j.HasKey("Appointment", "Diagnosis").HasName("pkey_appointment_diagnosis");
                        j.ToTable("appointment_diagnosis");
                        j.IndexerProperty<int>("Appointment").HasColumnName("appointment");
                        j.IndexerProperty<int>("Diagnosis").HasColumnName("diagnosis");
                    });

            entity.HasMany(d => d.Equipment).WithMany(p => p.Appointments)
                .UsingEntity<Dictionary<string, object>>(
                    "AppointmentEquipment",
                    r => r.HasOne<Equipment>().WithMany()
                        .HasForeignKey("Equipment")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fkey_equipment"),
                    l => l.HasOne<Appointment>().WithMany()
                        .HasForeignKey("Appointment")
                        .HasConstraintName("fkey_appointment"),
                    j =>
                    {
                        j.HasKey("Appointment", "Equipment").HasName("appointment_equipments_pkey");
                        j.ToTable("appointment_equipments");
                        j.IndexerProperty<int>("Appointment").HasColumnName("appointment");
                        j.IndexerProperty<int>("Equipment").HasColumnName("equipment");
                    });

            entity.HasMany(d => d.Services).WithMany(p => p.Appointments)
                .UsingEntity<Dictionary<string, object>>(
                    "AppointmentService",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("Service")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fkey_services"),
                    l => l.HasOne<Appointment>().WithMany()
                        .HasForeignKey("Appointment")
                        .HasConstraintName("fkey_appointments"),
                    j =>
                    {
                        j.HasKey("Appointment", "Service").HasName("pkey");
                        j.ToTable("appointment_services");
                        j.IndexerProperty<int>("Appointment").HasColumnName("appointment");
                        j.IndexerProperty<int>("Service").HasColumnName("service");
                    });
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasKey(e => e.DiagnosisId).HasName("diagnosis_pkey");

            entity.ToTable("diagnosis");

            entity.Property(e => e.DiagnosisId)
                .HasDefaultValueSql("nextval('diagnosis_id_seq'::regclass)")
                .HasColumnName("diagnosis_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DiagnosisName).HasColumnName("diagnosis_name");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("equipments_pkey");

            entity.ToTable("equipments");

            entity.Property(e => e.EquipmentId)
                .HasDefaultValueSql("nextval('equipments_id_seq'::regclass)")
                .HasColumnName("equipment_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EquipmentName).HasColumnName("equipment_name");
            entity.Property(e => e.ExpirationDate).HasColumnName("expiration_date");
            entity.Property(e => e.Supplier).HasColumnName("supplier");
        });

        modelBuilder.Entity<MedicalCheckupPlan>(entity =>
        {
            entity.HasKey(e => e.MedicalCheckupId).HasName("medical_checkup_plans_pkey");

            entity.ToTable("medical_checkup_plans");

            entity.Property(e => e.MedicalCheckupId).HasColumnName("medical_checkup_id");
            entity.Property(e => e.CheckupType).HasColumnName("checkup_type");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsCompleted)
                .HasDefaultValue(false)
                .HasColumnName("is_completed");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.CheckupTypeNavigation).WithMany(p => p.MedicalCheckupPlans)
                .HasForeignKey(d => d.CheckupType)
                .HasConstraintName("fk_checkup_type");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalCheckupPlans)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_patient");

            entity.HasMany(d => d.Appointments).WithMany(p => p.CheckupPlans)
                .UsingEntity<Dictionary<string, object>>(
                    "MedicalCheckupAppointment",
                    r => r.HasOne<Appointment>().WithMany()
                        .HasForeignKey("AppointmentId")
                        .HasConstraintName("fk_appointment"),
                    l => l.HasOne<MedicalCheckupPlan>().WithMany()
                        .HasForeignKey("CheckupPlanId")
                        .HasConstraintName("fk_checkup_plan"),
                    j =>
                    {
                        j.HasKey("CheckupPlanId", "AppointmentId").HasName("medical_checkup_appointments_pkey");
                        j.ToTable("medical_checkup_appointments");
                        j.IndexerProperty<int>("CheckupPlanId").HasColumnName("checkup_plan_id");
                        j.IndexerProperty<int>("AppointmentId").HasColumnName("appointment_id");
                    });
        });

        modelBuilder.Entity<MedicalCheckupType>(entity =>
        {
            entity.HasKey(e => e.MedicalCheckupTypeId).HasName("medical_checkup_types_pkey");

            entity.ToTable("medical_checkup_types");

            entity.Property(e => e.MedicalCheckupTypeId).HasColumnName("medical_checkup_type_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.MedicalCheckupName).HasColumnName("medical_checkup_name");
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.HasKey(e => e.OfficeId).HasName("offices_pkey");

            entity.ToTable("offices");

            entity.Property(e => e.OfficeId)
                .HasDefaultValueSql("nextval('offices_id_seq'::regclass)")
                .HasColumnName("office_id");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Offices)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey _room_type");

            entity.HasMany(d => d.Equipment).WithMany(p => p.Offices)
                .UsingEntity<Dictionary<string, object>>(
                    "OfficeEquipment",
                    r => r.HasOne<Equipment>().WithMany()
                        .HasForeignKey("Equipment")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fkey_equipment"),
                    l => l.HasOne<Office>().WithMany()
                        .HasForeignKey("Office")
                        .HasConstraintName("office_equipment_office_fkey"),
                    j =>
                    {
                        j.HasKey("Office", "Equipment").HasName("pkey_office_equipment");
                        j.ToTable("office_equipment");
                        j.IndexerProperty<int>("Office").HasColumnName("office");
                        j.IndexerProperty<int>("Equipment").HasColumnName("equipment");
                    });
        });

        modelBuilder.Entity<OfficeType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("types_pkey");

            entity.ToTable("office_types");

            entity.Property(e => e.TypeId)
                .HasDefaultValueSql("nextval('office_types_id_seq'::regclass)")
                .HasColumnName("type_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.TypeName).HasColumnName("type_name");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("patients_pkey");

            entity.ToTable("patients");

            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .HasColumnName("gender");
            entity.Property(e => e.InsuranceId)
                .HasPrecision(8)
                .HasColumnName("insurance_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.Phone)
                .HasPrecision(11)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Purpose>(entity =>
        {
            entity.HasKey(e => e.PurposeId).HasName("purposes_pkey");

            entity.ToTable("purposes");

            entity.Property(e => e.PurposeId)
                .ValueGeneratedNever()
                .HasColumnName("purpose_id");
            entity.Property(e => e.PurposeName).HasColumnName("purpose_name");
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.HasKey(e => e.ReceiptId).HasName("pkey_receipt");

            entity.ToTable("receipts");

            entity.Property(e => e.ReceiptId).HasColumnName("receipt_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.SpecialistId).HasColumnName("specialist_id");
            entity.Property(e => e.TotalSummary)
                .HasColumnType("money")
                .HasColumnName("total_summary");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_appointment");

            entity.HasOne(d => d.Specialist).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.SpecialistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_specialist");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("services_pkey");

            entity.ToTable("services");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("'no_description'::text")
                .HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.Specialist).HasColumnName("specialist");

            entity.HasOne(d => d.SpecialistNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.Specialist)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("specialists");
        });

        modelBuilder.Entity<Specialist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("specialists_pkey");

            entity.ToTable("specialists");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.Phone)
                .HasPrecision(11)
                .HasColumnName("phone");
            entity.Property(e => e.Specialization).HasColumnName("specialization");
            entity.Property(e => e.SpecializationType).HasColumnName("specialization_type");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Specialist)
                .HasForeignKey<Specialist>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_user");

            entity.HasOne(d => d.SpecializationNavigation).WithMany(p => p.Specialists)
                .HasForeignKey(d => d.Specialization)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("specialization");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("specialization_pkey");

            entity.ToTable("specializations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Specialization1)
                .HasMaxLength(80)
                .HasColumnName("specialization");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(false)
                .HasColumnName("is_active");
            entity.Property(e => e.IsSuperuser)
                .HasDefaultValue(false)
                .HasColumnName("is_superuser");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Salt).HasColumnName("salt");
            entity.Property(e => e.UserName).HasColumnName("user_name");
        });
        modelBuilder.HasSequence("diagnosis_id_seq");
        modelBuilder.HasSequence("equipments_id_seq");
        modelBuilder.HasSequence("office_types_id_seq");
        modelBuilder.HasSequence("offices_id_seq");
        modelBuilder.HasSequence("purposes_id_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
