using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvaloniaPrivateClinic.Models;

public class Diagnosis
{
    public int DiagnosisId { get; set; }

    public string? DiagnosisName { get; set; }

    public string Description { get; set; } = null!;

    [NotMapped]
    public string ToString
    {
        get => $"{DiagnosisName} - {Description}";
        private set { }
    }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}