using System;
using System.Collections.Generic;

namespace MindValley.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    // Store the hashed password (not the plain text password)
    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<MoodLog> MoodLogs { get; set; } = new List<MoodLog>();
}
