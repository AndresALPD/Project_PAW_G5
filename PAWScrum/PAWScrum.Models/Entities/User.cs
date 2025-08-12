using System;
using System.Collections.Generic;

namespace PAWScrum.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = string.Empty;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Role { get; set; }

    public virtual ICollection<Projects> Projects { get; set; } = new List<Projects>();

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

    public virtual ICollection<SprintBacklogItem> SprintBacklogItems { get; set; } = new List<SprintBacklogItem>();

    public virtual ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();
}
