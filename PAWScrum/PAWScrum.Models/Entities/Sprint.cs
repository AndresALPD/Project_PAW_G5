using System;
using System.Collections.Generic;

namespace PAWScrum.Models;

public partial class Sprint
{
    public int SprintId { get; set; }

    public int ProjectId { get; set; }

    public string? Name { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Goal { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<SprintBacklogItem> SprintBacklogItems { get; set; } = new List<SprintBacklogItem>();
}
