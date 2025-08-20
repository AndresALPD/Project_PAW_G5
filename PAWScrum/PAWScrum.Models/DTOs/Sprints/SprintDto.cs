using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.Sprints
{
    public class SprintDto
    {
        public int SprintId { get; set; }
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Goal { get; set; }
    }
}
