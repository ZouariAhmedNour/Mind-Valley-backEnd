using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MindValley.Models;

public partial class MoodLog
{
    public int MoodLogId { get; set; }

    public int UserId { get; set; }

    public string Mood { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime? LogDate { get; set; }

    // ✅ Ignore during deserialization
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}

public class MoodLogDto
{
    public int UserId { get; set; }
    public string Mood { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTime? LogDate { get; set; }
}
