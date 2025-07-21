using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PAWScrum.Business
{
    public class ProjectBusiness : IProjectBusiness
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectBusiness(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Projects>> GetAllAsync(bool includeOwner = true)
        {
            return await _projectRepository.GetAllAsync(includeOwner);
        }

        public async Task<Projects?> GetByIdAsync(int id, bool includeOwner = true)
        {
            return await _projectRepository.GetByIdAsync(id, includeOwner);
        }

        public async Task<bool> CreateAsync(Projects project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            return await _projectRepository.CreateAsync(project);
        }

        public async Task<bool> UpdateAsync(Projects project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            return await _projectRepository.UpdateAsync(project);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _projectRepository.DeleteAsync(id);
        }
    }
}