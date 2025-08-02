using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;
using PAWScrum.Models.DTOs;

namespace PAWScrum.Business.Interfaces
{
    public interface ISprintBusiness
    {
        Task<Sprints?> GetByIdAsync(int id);
        Task<IEnumerable<Sprints>> GetAllAsync();
        Task<bool> CreateAsync(Sprints sprint);
        Task<bool> UpdateAsync(Sprints sprint);
        Task<bool> DeleteAsync(int id);
    }
}
