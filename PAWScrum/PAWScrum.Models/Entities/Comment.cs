using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWScrum.Models.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string? Content { get; set; }
        [Required]
        [MaxLength(1000)]                 
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserTask Task { get; set; } = default!;
        public User User { get; set; } = default!;
    }
}
