using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;

namespace PAWScrum.Services.Interfaces
{
    public interface ISprintService
    {
        Task<Sprints?> GetByIdAsync(int id);
        Task<IEnumerable<Sprints>> GetAllAsync();
        Task<bool> CreateAsync(Sprints sprint);
        Task<bool> UpdateAsync(Sprints sprint);
        Task<bool> DeleteAsync(int id);
    }
}

