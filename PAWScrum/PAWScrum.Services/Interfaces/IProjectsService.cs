using PAWScrum.Models.DTOs.Projects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAWScrum.Business.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponseDto>> GetAllAsync(bool includeOwner = true);
        Task<ProjectResponseDto?> GetByIdAsync(int id, bool includeOwner = true);
        Task<ProjectResponseDto> CreateAsync(ProjectCreateDto projectDto);
        Task<bool> UpdateAsync(int id, ProjectUpdateDto projectDto);
        Task<bool> DeleteAsync(int id);
        
    }
}