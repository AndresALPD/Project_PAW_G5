using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.ActivityLog
{
    public class ActivityLogResponseDto
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
