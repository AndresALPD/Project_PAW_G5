using System;
using System.Collections.Generic;

namespace PAWScrum.Models;

public partial class ProductBacklogItem
{
    public int ItemId { get; set; }

    public int ProjectId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? Priority { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Projects Project { get; set; } = null!;

    public virtual ICollection<SprintBacklogItem> SprintBacklogItems { get; set; } = new List<SprintBacklogItem>();
}
