using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Architecture.Helpers;
using PAWScrum.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Models.DTOs;
using PAWScrum.Data.Context;

namespace PAWScrum.Business.Managers
{
    public class SprintBacklogBusiness : ISprintBacklogBusiness
    {
        private readonly ISprintBacklogRepository _repository;

        public SprintBacklogBusiness(ISprintBacklogRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SprintBacklogItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<SprintBacklogItem?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> CreateAsync(SprintBacklogItem item)
        {
            await _repository.AddAsync(item);
            return true;
        }

        public async Task<bool> UpdateAsync(SprintBacklogItem item)
        {
            await _repository.UpdateAsync(item);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<List<SprintBacklogItem>> GetBySprintIdAsync(int sprintId)
        {
            var items = await _repository.GetAllAsync();
            return items.Where(x => x.SprintId == sprintId).ToList();
        }
    }
}
