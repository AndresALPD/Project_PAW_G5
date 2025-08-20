using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.Projects;
using PAWScrum.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAWScrum.Business.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync(bool includeOwner = true)
        {
            var projects = await _projectRepository.GetAllAsync(includeOwner);
            return projects.Select(p => new ProjectResponseDto
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                ProjectKey = p.ProjectKey,
                Description = p.Description,
                OwnerId = p.OwnerId,
                Visibility = p.Visibility,
                Status = p.Status,
                StartDate = p.StartDate.HasValue ? p.StartDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                EndDate = p.EndDate.HasValue ? p.EndDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                SprintDuration = p.SprintDuration,
                RepositoryUrl = p.RepositoryUrl,
                IsArchived = p.IsArchived,
                CreatedDate = p.CreatedDate
            });
        }

        public async Task<ProjectResponseDto?> GetByIdAsync(int id, bool includeOwner = true)
        {
            var project = await _projectRepository.GetByIdAsync(id, includeOwner);
            if (project == null) return null;

            return new ProjectResponseDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ProjectKey = project.ProjectKey,
                Description = project.Description,
                OwnerId = project.OwnerId,
                Visibility = project.Visibility,
                Status = project.Status,
                StartDate = project.StartDate.HasValue ? project.StartDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                EndDate = project.EndDate.HasValue ? project.EndDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                SprintDuration = project.SprintDuration,
                RepositoryUrl = project.RepositoryUrl,
                IsArchived = project.IsArchived,
                CreatedDate = project.CreatedDate
            };
        }

        public async Task<ProjectResponseDto> CreateAsync(ProjectCreateDto projectDto)
        {
            var project = new Project
            {
                ProjectName = projectDto.ProjectName,
                ProjectKey = projectDto.ProjectKey,
                Description = projectDto.Description,
                OwnerId = projectDto.OwnerId,
                Visibility = projectDto.Visibility ?? "Private",
                Status = projectDto.Status ?? "Active",
                StartDate = projectDto.StartDate.HasValue ?
                    DateOnly.FromDateTime(projectDto.StartDate.Value) : (DateOnly?)null,
                EndDate = projectDto.EndDate.HasValue ?
                    DateOnly.FromDateTime(projectDto.EndDate.Value) : (DateOnly?)null,
                SprintDuration = projectDto.SprintDuration,
                RepositoryUrl = projectDto.RepositoryUrl,
                CreatedDate = DateTime.UtcNow
            };

            await _projectRepository.CreateAsync(project);

            return new ProjectResponseDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ProjectKey = project.ProjectKey,
                Description = project.Description,
                OwnerId = project.OwnerId,
                Visibility = project.Visibility,
                Status = project.Status,
                StartDate = project.StartDate.HasValue ?
                    project.StartDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                EndDate = project.EndDate.HasValue ?
                    project.EndDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                SprintDuration = project.SprintDuration,
                RepositoryUrl = project.RepositoryUrl,
                IsArchived = project.IsArchived,
                CreatedDate = project.CreatedDate
            };
        }

        public async Task<bool> UpdateAsync(int id, ProjectUpdateDto projectDto)
        {
            var project = await _projectRepository.GetByIdAsync(id, false);
            if (project == null) return false;

            if (projectDto.ProjectName != null)
                project.ProjectName = projectDto.ProjectName;

            if (projectDto.Description != null)
                project.Description = projectDto.Description;

            if (projectDto.Visibility != null)
                project.Visibility = projectDto.Visibility;

            if (projectDto.Status != null)
                project.Status = projectDto.Status;

            if (projectDto.StartDate.HasValue)
                project.StartDate = DateOnly.FromDateTime(projectDto.StartDate.Value);

            if (projectDto.EndDate.HasValue)
                project.EndDate = DateOnly.FromDateTime(projectDto.EndDate.Value);

            if (projectDto.SprintDuration.HasValue)
                project.SprintDuration = projectDto.SprintDuration.Value;

            if (projectDto.RepositoryUrl != null)
                project.RepositoryUrl = projectDto.RepositoryUrl;

            if (projectDto.IsArchived.HasValue)
                project.IsArchived = projectDto.IsArchived.Value;

            return await _projectRepository.UpdateAsync(project);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _projectRepository.DeleteAsync(id);
        }
    }
}