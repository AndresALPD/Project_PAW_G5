using System;
using System.Collections.Generic;

namespace PAWScrum.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int UserId { get; set; }

    public int? SprintItemId { get; set; }

    public int? TaskId { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual SprintBacklogItem? SprintItem { get; set; }

    public virtual UserTask? Task { get; set; }

    public virtual User User { get; set; } = null!;
}
