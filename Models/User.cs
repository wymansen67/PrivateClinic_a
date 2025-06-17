using System;
using System.Collections.Generic;

namespace AvaloniaPrivateClinic.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsSuperuser { get; set; }

    public virtual Specialist? Specialist { get; set; }
}
