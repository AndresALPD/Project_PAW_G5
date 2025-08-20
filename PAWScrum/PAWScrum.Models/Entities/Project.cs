using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PAWScrum.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    [Required(ErrorMessage = "El nombre del proyecto es obligatorio")]
    [StringLength(200)]
    public string ProjectName { get; set; } = null!;

    [Required]
    [StringLength(10)]
    public string ProjectKey { get; set; } = null!; // Mantener de QA

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public int OwnerId { get; set; }

    [StringLength(20)]
    public string Visibility { get; set; } = "Private";

    [StringLength(20)]
    public string Status { get; set; } = "Active";

    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public int SprintDuration { get; set; } = 14; // Mantener de QA

    [Url]
    [StringLength(200)]
    public string? RepositoryUrl { get; set; }

    public bool IsArchived { get; set; } = false; // No nullable como QA

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Mantener de QA

    // Relaciones
    public virtual User Owner { get; set; } = null!;

    // Colecciones - usar nombres plurales consistentes
    public virtual ICollection<Sprint> Sprints { get; set; } = new List<Sprint>();
    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
    public virtual ICollection<ProductBacklogItem> ProductBacklogItems { get; set; } = new List<ProductBacklogItem>();
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
}