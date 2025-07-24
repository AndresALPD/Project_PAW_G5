using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.Comments
{
    public class CommentCreateDto
    {
        public string Content { get; set; }
        public int WorkTaskId { get; set; }
        public int UserId { get; set; }
    }
}
