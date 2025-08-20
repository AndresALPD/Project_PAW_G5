using PAWScrum.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWScrum.Models;

public partial class SprintBacklogItem
{
    [Key]
    public int SprintItemId { get; set; }

    public int SprintId { get; set; }

    public int ProductBacklogItemId { get; set; }

    public int? AssignedTo { get; set; }

    public string? Status { get; set; }

    public decimal? EstimationHours { get; set; }

    public decimal? CompletedHours { get; set; }

    [ForeignKey(nameof(AssignedTo))] // Mantener de QA (importante para CRUD)
    public virtual User? AssignedToNavigation { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ProductBacklogItem ProductBacklogItem { get; set; } = null!;

    public virtual Sprint Sprint { get; set; } = null!; // Cambiar a Sprint singular

    public virtual ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();
}