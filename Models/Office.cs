using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvaloniaPrivateClinic.Models;

public partial class Office
{
    public int OfficeId { get; set; }

    public string? Number { get; set; }

    public int Type { get; set; }
    
    [NotMapped]
    public string ToString
    {
        get
        {
            if (TypeNavigation != null)
            {
                return $"{Number} - {TypeNavigation.TypeName}";
            }
            else
            {
                return $"{Number} - {Type}";
            }
        }
    }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual OfficeType TypeNavigation { get; set; } = null!;

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
}
