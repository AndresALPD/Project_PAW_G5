using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.Projects
{

    // ProjectResponseDto.cs
    public class ProjectResponseDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectKey { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public string Visibility { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int SprintDuration { get; set; }
        public string RepositoryUrl { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
