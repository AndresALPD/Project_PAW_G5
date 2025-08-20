using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.ActivityLog
{
    public class ActivityLogCreateDto
    {
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }

        [Required]
        public string Action { get; set; } = null!;

        public DateTime? Timestamp { get; set; }
    }
}

