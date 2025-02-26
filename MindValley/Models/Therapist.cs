using System;
using System.Collections.Generic;

namespace MindValley.Models;

public partial class Therapist
{
    public int TherapistId { get; set; }

    public string Name { get; set; } = null!;

    public string? Specialization { get; set; }

    public string? ContactInfo { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
