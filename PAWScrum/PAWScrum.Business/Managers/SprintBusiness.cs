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
        private readonly ISprintRepository _sprintRepository;

        public SprintBusiness(ISprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        public async Task<Sprints?> GetByIdAsync(int id)
            => await _sprintRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Sprints>> GetAllAsync()
            => await _sprintRepository.GetAllAsync();

        public async Task<bool> CreateAsync(Sprints sprint)
            => await _sprintRepository.CreateAsync(sprint);

        public async Task<bool> UpdateAsync(Sprints sprint)
            => await _sprintRepository.UpdateAsync(sprint);

        public async Task<bool> DeleteAsync(int id)
            => await _sprintRepository.DeleteAsync(id);
    }
}
