using System;
using System.Collections.Generic;

namespace AvaloniaPrivateClinic.Models;

public partial class OfficeType
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Office> Offices { get; set; } = new List<Office>();
}
