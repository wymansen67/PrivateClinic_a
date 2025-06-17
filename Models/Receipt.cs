using System;
using System.Collections.Generic;

namespace AvaloniaPrivateClinic.Models;

public partial class Receipt
{
    public int ReceiptId { get; set; }

    public int AppointmentId { get; set; }

    public int SpecialistId { get; set; }

    public decimal TotalSummary { get; set; }

    public DateOnly Date { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Specialist Specialist { get; set; } = null!;
}
