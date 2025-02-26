using System;
using System.Collections.Generic;

namespace MindValley.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int? UserId { get; set; }

    public int? TherapistId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string? Notes { get; set; }

    public virtual Therapist? Therapist { get; set; } 

    public virtual User? User { get; set; } 
}
