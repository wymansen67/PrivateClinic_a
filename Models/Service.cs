using System;
using System.Collections.Generic;

namespace AvaloniaPrivateClinic.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Specialist { get; set; }

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Specialist SpecialistNavigation { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
