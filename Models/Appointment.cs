using System;
using System.Collections.Generic;

namespace AvaloniaPrivateClinic.Models;

public partial class Appointment
{
    public int AppointmentNumber { get; set; }

    public DateOnly Date { get; set; }

    public int Patient { get; set; }

    public int Purpose { get; set; }

    public int Office { get; set; }

    public decimal TotalPrice { get; set; }

    public short? Discount { get; set; }

    public string? Commentaries { get; set; }

    public bool IsPlanned { get; set; }

    public int Specialist { get; set; }

    public TimeOnly? Time { get; set; }

    public virtual Office OfficeNavigation { get; set; } = null!;

    public virtual Patient PatientNavigation { get; set; } = null!;

    public virtual Purpose PurposeNavigation { get; set; } = null!;

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual Specialist SpecialistNavigation { get; set; } = null!;

    public virtual ICollection<MedicalCheckupPlan> CheckupPlans { get; set; } = new List<MedicalCheckupPlan>();

    public virtual ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
