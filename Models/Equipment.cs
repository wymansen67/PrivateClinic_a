using System;
using System.Collections.Generic;

namespace AvaloniaPrivateClinic.Models;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public string? EquipmentName { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public int? Supplier { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Office> Offices { get; set; } = new List<Office>();
}
