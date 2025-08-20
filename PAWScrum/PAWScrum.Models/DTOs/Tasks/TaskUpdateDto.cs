using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.Tasks
{
    public class TaskUpdateDto : TaskCreateDto
    {
        public decimal CompletedHours { get; set; } 
        public string Status { get; set; }
    }
}
