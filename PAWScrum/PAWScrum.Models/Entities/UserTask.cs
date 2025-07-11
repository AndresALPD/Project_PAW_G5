using System;
using System.Collections.Generic;

namespace PAWScrum.Models;

public partial class UserTask
{
    public int TaskId { get; set; }

    public int SprintItemId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public int? AssignedTo { get; set; }

    public decimal? EstimationHours { get; set; }

    public decimal? CompletedHours { get; set; }

    public virtual User? AssignedToNavigation { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual SprintBacklogItem SprintItem { get; set; } = null!;
}
