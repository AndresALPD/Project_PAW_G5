using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models;
using PAWScrum.Models.DTOs;

namespace PAWScrum.Business.Interfaces
{
        public interface ISprintBacklogBusiness
        {
            Task<List<SprintBacklogItem>> GetAllAsync();
            Task<SprintBacklogItem?> GetByIdAsync(int id);
            Task<List<SprintBacklogItem>> GetBySprintIdAsync(int sprintId);
            Task<bool> CreateAsync(SprintBacklogItem item);
            Task<bool> UpdateAsync(SprintBacklogItem item);
            Task<bool> DeleteAsync(int id);
    }
    }

