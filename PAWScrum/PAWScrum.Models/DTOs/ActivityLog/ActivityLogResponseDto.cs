using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.ActivityLog
{
    public class ActivityLogResponseDto
    {
        public int LogId { get; set; }              // <- se expone tal cual
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
