using System;
using System.Collections.Generic;

namespace MindValley.Models;

public partial class Resource
{
    public int ResourceId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Type { get; set; } = null!;

    public string Url { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
