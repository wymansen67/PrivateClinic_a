using System.Collections.Generic;

namespace AvaloniaPrivateClinic.Models;

public class Purpose
{
    public int PurposeId { get; set; }

    public string? PurposeName { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}