using System;
using System.Collections.Generic;

namespace PAWScrum.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    public int OwnerId { get; set; }

    public string? Visibility { get; set; }

    public string? Status { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? RepositoryUrl { get; set; }

    public bool? IsArchived { get; set; }

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<ProductBacklogItem> ProductBacklogItems { get; set; } = new List<ProductBacklogItem>();

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

    public virtual ICollection<Sprint> Sprints { get; set; } = new List<Sprint>();
}
