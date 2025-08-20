using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWScrum.Models;

public partial class ActivityLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LogId { get; set; }  // Usar LogId de QA (coincide con DbContext)

    public int? UserId { get; set; }  // Hacer nullable como BranchPao

    public int? ProjectId { get; set; }  // Mantener nullable

    [Required]
    public string Action { get; set; } = string.Empty;  // Usar inicialización de BranchPao

    public DateTime Timestamp { get; set; }  // Hacer required como BranchPao

    // Relaciones usando los nombres de clase corregidos (Project, no Projects)
    public virtual Project? Project { get; set; }  // Usar Project singular

    public virtual User? User { get; set; }  // Hacer nullable como BranchPao
}