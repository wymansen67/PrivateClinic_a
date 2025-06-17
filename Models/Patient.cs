using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvaloniaPrivateClinic.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public decimal InsuranceId { get; set; }

    public DateOnly Birthday { get; set; }

    public char Gender { get; set; }

    public decimal Phone { get; set; }

    public string Address { get; set; } = null!;
    
    [NotMapped]
    public string ToString
    {
        get
        {
            return $"{LastName} {FirstName} {MiddleName}";
        }
    }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<MedicalCheckupPlan> MedicalCheckupPlans { get; set; } = new List<MedicalCheckupPlan>();
}
