using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvaloniaPrivateClinic.Models;

public class Specialist
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int Specialization { get; set; }

    public string SpecializationType { get; set; } = null!;

    public decimal Phone { get; set; }

    [NotMapped]
    public string ToString
    {
        get
        {
            if (SpecializationNavigation != null)
                return
                    $"{LastName} {FirstName.Substring(0, 1)}. {MiddleName.Substring(0, 1)}. - {SpecializationNavigation.Specialization1}";

            return $"{LastName} {FirstName.Substring(0, 1)}. {MiddleName.Substring(0, 1)}.";
        }
    }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual User IdNavigation { get; set; } = null!;

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual Specialization SpecializationNavigation { get; set; } = null!;
}