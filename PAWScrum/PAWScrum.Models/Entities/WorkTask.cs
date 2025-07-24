using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.Entities
{
    public class WorkTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int HoursEstimated { get; set; }
        public int HoursCompleted { get; set; }
        public int ProductBacklogItemId { get; set; } // HU
        public ProductBacklogItem ProductBacklogItem { get; set; } // Relación
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
