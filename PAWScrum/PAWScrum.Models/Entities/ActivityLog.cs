using System;
using System.Collections.Generic;

namespace PAWScrum.Models;

public partial class ActivityLog
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public int? ProjectId { get; set; }

    public string? Action { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual Project? Project { get; set; }

    public virtual User User { get; set; } = null!;
}
