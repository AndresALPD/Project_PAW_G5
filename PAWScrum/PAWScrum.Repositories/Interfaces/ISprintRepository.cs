using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Repositories.Implementations
{
    public interface ISprintRepository
    {
        Task<IEnumerable<Sprints>> GetAllAsync();
        Task<Sprints?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Sprints sprint);
        Task<bool> UpdateAsync(Sprints sprint);
        Task<bool> DeleteAsync(int id);

    }
}