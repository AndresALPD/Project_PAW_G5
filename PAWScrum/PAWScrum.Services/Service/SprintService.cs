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
    public class SprintService : ISprintService
    {
        private readonly ISprintBusiness _sprintBusiness;

        public SprintService(ISprintBusiness sprintBusiness)
        {
            _sprintBusiness = sprintBusiness;
        }

        public async Task<Sprints?> GetByIdAsync(int id)
        {
            return await _sprintBusiness.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Sprints>> GetAllAsync()
        {
            return await _sprintBusiness.GetAllAsync();
        }

        public async Task<bool> CreateAsync(Sprints sprint)
        {
            return await _sprintBusiness.CreateAsync(sprint);
        }

        public async Task<bool> UpdateAsync(Sprints sprint)
        {
            return await _sprintBusiness.UpdateAsync(sprint);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _sprintBusiness.DeleteAsync(id);
        }
    }
}
