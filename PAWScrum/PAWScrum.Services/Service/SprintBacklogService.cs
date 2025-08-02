using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Services.Service
{
    public class SprintBacklogService : ISprintBacklogService
    {
        private readonly ISprintBacklogBusiness _sprintBacklogBusiness;

        public SprintBacklogService(ISprintBacklogBusiness sprintBacklogBusiness)
        {
            _sprintBacklogBusiness = sprintBacklogBusiness;
        }

        public async Task<IEnumerable<SprintBacklogItem>> GetAllAsync()
        {
            return await _sprintBacklogBusiness.GetAllAsync();
        }

        public async Task<SprintBacklogItem?> GetByIdAsync(int id)
        {
            return await _sprintBacklogBusiness.GetByIdAsync(id);
        }

        public async Task<IEnumerable<SprintBacklogItem>> GetBySprintIdAsync(int sprintId)
        {
            return await _sprintBacklogBusiness.GetBySprintIdAsync(sprintId);
        }

        public async Task<bool> CreateAsync(SprintBacklogItem item)
        {
            return await _sprintBacklogBusiness.CreateAsync(item);
        }

        public async Task<bool> UpdateAsync(SprintBacklogItem item)
        {
            return await _sprintBacklogBusiness.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _sprintBacklogBusiness.DeleteAsync(id);
        }
    }
}
