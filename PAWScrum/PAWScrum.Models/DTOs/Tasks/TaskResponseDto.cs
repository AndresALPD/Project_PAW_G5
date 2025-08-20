using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.Tasks
{
    public class TaskResponseDto
    {
        public int Id { get; set; }                    
        public string Title { get; set; }
        public string Description { get; set; }
        public int SprintItemId { get; set; }
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }
        public decimal EstimationHours { get; set; }
        public decimal CompletedHours { get; set; }
        public string Status { get; set; }
    }
}
