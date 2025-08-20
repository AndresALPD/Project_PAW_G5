using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.ProductBacklog
{
    public class ProductBacklogCreateDto
    {
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Priority { get; set; } = 5; // 5 es el máw alto, y 1 es la baja prioridad
        public string Status { get; set; } = "To Do"; // to do , in progress, done
    }
}
