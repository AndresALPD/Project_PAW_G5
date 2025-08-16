using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.SprintBacklog
{
    public class SprintBacklogDto
    {
        public int SprintItemId { get; set; }
        public int SprintId { get; set; }
        public int ProductBacklogItemId { get; set; }
        public int? AssignedTo { get; set; }
        public string Status { get; set; } = "To Do";
        public decimal? EstimationHours { get; set; }
        public decimal? CompletedHours { get; set; }
    }
}
