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
    public class SprintBusiness : ISprintBusiness
    {
        private readonly ISprintRepository _repository;

        public SprintBusiness(ISprintRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Sprints>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Sprints?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<bool> CreateAsync(Sprints sprint) => await _repository.CreateAsync(sprint);
        public async Task<bool> UpdateAsync(Sprints sprint) => await _repository.UpdateAsync(sprint);
        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
