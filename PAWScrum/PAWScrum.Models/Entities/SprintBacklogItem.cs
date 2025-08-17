using System;
using System.Collections.Generic;
using PAWScrum.Models.Entities;

namespace PAWScrum.Models;

public partial class SprintBacklogItem
{
    public int SprintItemId { get; set; }
    public int SprintId { get; set; }
    public int ProductBacklogItemId { get; set; }
    public int? AssignedTo { get; set; }
    public string? Status { get; set; }
    public decimal? EstimationHours { get; set; }
    public decimal? CompletedHours { get; set; }
    public virtual User? AssignedToNavigation { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ProductBacklogItem ProductBacklogItem { get; set; } = null!;
    public virtual Sprint Sprint { get; set; } = null!;
    public ICollection<UserTask> Tasks { get; set; } = new HashSet<UserTask>();
}
