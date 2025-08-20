using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWScrum.Models.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }

        public int? TaskId { get; set; }                 
        public int? SprintItemId { get; set; }          
        public int UserId { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public User User { get; set; } = default!;
        public UserTask? Task { get; set; }              
        [ForeignKey(nameof(SprintItemId))]
        public SprintBacklogItem? SprintItem { get; set; }
    }
}
