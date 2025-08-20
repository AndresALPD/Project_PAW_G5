using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWScrum.Models.Entities;

public class ActivityLog
{
    public int ActivityId { get; set; }
    public int? UserId { get; set; }
    public int? ProjectId { get; set; }
    public string Action { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    public virtual User? User { get; set; }
    public virtual Project? Project { get; set; }
}
