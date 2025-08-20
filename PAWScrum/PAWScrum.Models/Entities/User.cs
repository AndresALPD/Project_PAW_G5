using System;
using System.Collections.Generic;
using PAWScrum.Models.Entities;

namespace PAWScrum.Models;

public partial class User
{
    public User()
    {
        Projects = new HashSet<Project>();
        ProjectMembers = new HashSet<ProjectMember>();
        ActivityLogs = new HashSet<ActivityLog>();
        Tasks = new HashSet<UserTask>();
        SprintBacklogItems = new HashSet<SprintBacklogItem>();
        Comments = new HashSet<Comment>();
    }

    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Project> Projects { get; set; }
    public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
    public virtual ICollection<UserTask> Tasks { get; set; }
    public virtual ICollection<SprintBacklogItem> SprintBacklogItems { get; set; }
}