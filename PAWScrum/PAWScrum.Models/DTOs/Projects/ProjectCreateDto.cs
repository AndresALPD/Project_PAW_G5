// ProjectCreateDto.cs
using System.ComponentModel.DataAnnotations;

public class ProjectCreateDto
{
    [Required]
    [StringLength(200)]
    public string ProjectName { get; set; }

    [Required]
    [StringLength(10)]
    public string ProjectKey { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int OwnerId { get; set; }

    [StringLength(20)]
    public string Visibility { get; set; } = "Private";

    [StringLength(20)]
    public string Status { get; set; } = "Active";

    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Range(1, 30)]
    public int SprintDuration { get; set; } = 14;

    [Url]
    [StringLength(200)]
    public string RepositoryUrl { get; set; }
}

