using System;
using System.Collections.Generic;

namespace AvaloniaPrivateClinic.Models;

public partial class MedicalCheckupPlan
{
    public int MedicalCheckupId { get; set; }

    public int PatientId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool IsCompleted { get; set; }

    public int? CheckupType { get; set; }

    public string? Comment { get; set; }

    public virtual MedicalCheckupType? CheckupTypeNavigation { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
