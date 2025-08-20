using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.Comments
{
    public class CommentCreateDto
    {
        public int TaskId { get; set; }  
        public int UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
