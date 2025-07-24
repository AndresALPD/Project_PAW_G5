using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.Tasks
{
    public class TaskCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int HoursEstimated { get; set; }
        public int ProductBacklogItemId { get; set; }
    }
}
