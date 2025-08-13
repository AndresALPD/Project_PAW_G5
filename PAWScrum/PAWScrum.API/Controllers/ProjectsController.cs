using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.Projects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAWScrum.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectBusiness _projectBusiness;

        public ProjectsController(IProjectBusiness projectBusiness)
        {
            _projectBusiness = projectBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetAll(bool includeOwner = true)
        {
            try
            {
                var projects = await _projectBusiness.GetAllAsync(includeOwner);
                return Ok(MapToDtoList(projects, includeOwner));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseDto>> GetById(int id, bool includeOwner = true)
        {
            try
            {
                var project = await _projectBusiness.GetByIdAsync(id, includeOwner);
                if (project == null) return NotFound();
                return Ok(MapToDto(project, includeOwner));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> Create([FromBody] ProjectCreateDto projectDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var project = new Projects
                {
                    ProjectName = projectDto.ProjectName,
                    ProjectKey = projectDto.ProjectKey,
                    Description = projectDto.Description,
                    OwnerId = projectDto.OwnerId,
                    Visibility = projectDto.Visibility ?? "Private",
                    Status = projectDto.Status ?? "Active",
                    StartDate = projectDto.StartDate.HasValue ? 
                        DateOnly.FromDateTime(projectDto.StartDate.Value) : null,
                    EndDate = projectDto.EndDate.HasValue ? 
                        DateOnly.FromDateTime(projectDto.EndDate.Value) : null,
                    SprintDuration = projectDto.SprintDuration,
                    RepositoryUrl = projectDto.RepositoryUrl,
                    CreatedDate = DateTime.UtcNow
                };

                var created = await _projectBusiness.CreateAsync(project);
                if (!created) return StatusCode(500, "Failed to create project");

                return CreatedAtAction(nameof(GetById), 
                    new { id = project.ProjectId }, 
                    MapToDto(project));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectUpdateDto projectDto)
        {
            try
            {
                var existingProject = await _projectBusiness.GetByIdAsync(id, false);
                if (existingProject == null) return NotFound();

                if (projectDto.ProjectName != null)
                    existingProject.ProjectName = projectDto.ProjectName;

                if (projectDto.Description != null)
                    existingProject.Description = projectDto.Description;

                if (projectDto.Visibility != null)
                    existingProject.Visibility = projectDto.Visibility;

                if (projectDto.Status != null)
                    existingProject.Status = projectDto.Status;

                if (projectDto.StartDate.HasValue)
                    existingProject.StartDate = DateOnly.FromDateTime(projectDto.StartDate.Value);

                if (projectDto.EndDate.HasValue)
                    existingProject.EndDate = DateOnly.FromDateTime(projectDto.EndDate.Value);

                if (projectDto.SprintDuration.HasValue)
                    existingProject.SprintDuration = projectDto.SprintDuration.Value;

                if (projectDto.RepositoryUrl != null)
                    existingProject.RepositoryUrl = projectDto.RepositoryUrl;

                if (projectDto.IsArchived.HasValue)
                    existingProject.IsArchived = projectDto.IsArchived.Value;

                var updated = await _projectBusiness.UpdateAsync(existingProject);
                if (!updated) return StatusCode(500, "Failed to update project");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _projectBusiness.DeleteAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private ProjectResponseDto MapToDto(Projects project, bool includeOwner = true)
        {
            return new ProjectResponseDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ProjectKey = project.ProjectKey,
                Description = project.Description,
                OwnerId = project.OwnerId,    
                Visibility = project.Visibility,
                Status = project.Status,
                StartDate = project.StartDate?.ToDateTime(TimeOnly.MinValue),
                EndDate = project.EndDate?.ToDateTime(TimeOnly.MinValue),
                SprintDuration = project.SprintDuration,
                RepositoryUrl = project.RepositoryUrl,
                IsArchived = project.IsArchived,
                CreatedDate = project.CreatedDate
            };
        }

        private IEnumerable<ProjectResponseDto> MapToDtoList(IEnumerable<Projects> projects, bool includeOwner = true)
        {
            foreach (var project in projects)
            {
                yield return MapToDto(project, includeOwner);
            }
        }
    }
}