using System;
using System.Collections.Generic;

namespace AvaloniaPrivateClinic.Models;

public partial class MedicalCheckupType
{
    public int MedicalCheckupTypeId { get; set; }

    public string MedicalCheckupName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<MedicalCheckupPlan> MedicalCheckupPlans { get; set; } = new List<MedicalCheckupPlan>();
}
