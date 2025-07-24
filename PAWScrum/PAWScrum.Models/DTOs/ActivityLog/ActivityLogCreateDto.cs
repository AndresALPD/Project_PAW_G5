using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.ActivityLog
{
    public class ActivityLogCreateDto
    {
        public int UserId { get; set; }
        public int? ProjectId { get; set; }
        public string? Action { get; set; }
    }
}

