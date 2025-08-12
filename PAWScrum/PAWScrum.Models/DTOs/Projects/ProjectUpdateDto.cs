using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.Projects
{
    // ProjectUpdateDto.cs
    public class ProjectUpdateDto
    {
        [StringLength(200)]
        public string ProjectName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Visibility { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Range(1, 30)]
        public int? SprintDuration { get; set; }

        [Url]
        [StringLength(200)]
        public string RepositoryUrl { get; set; }

        public bool? IsArchived { get; set; }
    }

}
