using System;
using System.Collections.Generic;

namespace AvaloniaPrivateClinic.Models;

public partial class Specialization
{
    public int Id { get; set; }

    public string Specialization1 { get; set; } = null!;

    public virtual ICollection<Specialist> Specialists { get; set; } = new List<Specialist>();
}
