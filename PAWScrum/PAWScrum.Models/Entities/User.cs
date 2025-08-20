using PAWScrum.Models.Entities;
using System;
using System.Collections.Generic;

namespace PAWScrum.Models;

public partial class User
{
    public User()
    {
        // Inicialización explícita como BranchPao pero con List<> como QA
        Projects = new List<Project>();
        ProjectMembers = new List<ProjectMember>();
        ActivityLogs = new List<ActivityLog>();
        Tasks = new List<UserTask>();
        SprintBacklogItems = new List<SprintBacklogItem>();
        Comments = new List<Comment>();
    }

    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!; // No string.Empty como QA

    public string? FirstName { get; set; } // Hacer nullable como BranchPao

    public string? LastName { get; set; } // Hacer nullable como BranchPao

    public string Role { get; set; } = null!; // No nullable como BranchPao

    // Relaciones - usar Project singular y List<> como QA
    public virtual ICollection<Project> Projects { get; set; }

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; }

    public virtual ICollection<SprintBacklogItem> SprintBacklogItems { get; set; }

    public virtual ICollection<UserTask> Tasks { get; set; }
}