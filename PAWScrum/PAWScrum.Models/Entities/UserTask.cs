using System;
using System.Collections.Generic;
using PAWScrum.Models.Entities;

namespace PAWScrum.Models;

public class UserTask
{
    public int TaskId { get; set; }
    public string? Title { get; set; }
    public decimal EstimationHours { get; set; }
    public decimal CompletedHours { get; set; }
    public string? Status { get; set; }
    public int SprintItemId { get; set; }
    public SprintBacklogItem SprintItem { get; set; } = default!;
    public int? AssignedTo { get; set; }
    public User? AssignedToNavigation { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public string Description { get; set; }
}

