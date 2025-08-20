using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Data.Context;        // PAWScrumDbContext
using PAWScrum.Models.DTOs.Tasks;   // TaskCreateDto, TaskUpdateDto, TaskResponseDto
using PAWScrum.Services.Interfaces; // ITaskService

namespace PAWScrum.Services.Service
{
    public class TaskService : ITaskService
    {
        private readonly PAWScrumDbContext _db;

        public TaskService(PAWScrumDbContext db)
        {
            _db = db;
        }

        public async Task<List<TaskResponseDto>> GetAllAsync()
        {
            return await (
                from t in _db.Tasks
                join u in _db.Users on t.AssignedTo equals u.UserId into gj
                from u in gj.DefaultIfEmpty()
                select new TaskResponseDto
                {
                    Id = t.TaskId,
                    Title = t.Title ?? string.Empty,
                    Description = t.Description ?? string.Empty,
                    SprintItemId = t.SprintItemId,
                    AssignedUserId = t.AssignedTo,
                    AssignedUserName = u != null ? u.Username : null,
                    EstimationHours = t.EstimationHours,
                    CompletedHours = t.CompletedHours,
                    Status = t.Status ?? "To Do"
                }
            ).AsNoTracking().ToListAsync();
        }

        public async Task<TaskResponseDto> GetByIdAsync(int id)
        {
            return await (
                from t in _db.Tasks
                where t.TaskId == id
                join u in _db.Users on t.AssignedTo equals u.UserId into gj
                from u in gj.DefaultIfEmpty()
                select new TaskResponseDto
                {
                    Id = t.TaskId,
                    Title = t.Title ?? string.Empty,
                    Description = t.Description ?? string.Empty,
                    SprintItemId = t.SprintItemId,
                    AssignedUserId = t.AssignedTo,
                    AssignedUserName = u != null ? u.Username : null,
                    EstimationHours = t.EstimationHours,
                    CompletedHours = t.CompletedHours,
                    Status = t.Status ?? "To Do"
                }
            ).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TaskResponseDto> CreateAsync(TaskCreateDto dto)
        {
            var entry = await _db.Tasks.AddAsync(new()
            {
                Title = dto.Title,
                Description = dto.Description,
                SprintItemId = dto.SprintItemId,
                AssignedTo = dto.AssignedUserId,
                EstimationHours = dto.EstimationHours,
                CompletedHours = 0m,
                Status = "To Do"
            });

            await _db.SaveChangesAsync();

            var id = entry.Entity.TaskId;

            return await (
                from t in _db.Tasks
                where t.TaskId == id
                join u in _db.Users on t.AssignedTo equals u.UserId into gj
                from u in gj.DefaultIfEmpty()
                select new TaskResponseDto
                {
                    Id = t.TaskId,
                    Title = t.Title ?? string.Empty,
                    Description = t.Description ?? string.Empty,
                    SprintItemId = t.SprintItemId,
                    AssignedUserId = t.AssignedTo,
                    AssignedUserName = u != null ? u.Username : null,
                    EstimationHours = t.EstimationHours,
                    CompletedHours = t.CompletedHours,
                    Status = t.Status ?? "To Do"
                }
            ).FirstAsync();
        }

        public async Task<TaskResponseDto> UpdateAsync(int id, TaskUpdateDto dto)
        {
            var entity = await _db.Tasks.FirstOrDefaultAsync(t => t.TaskId == id);
            if (entity == null) return null;

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.SprintItemId = dto.SprintItemId;
            entity.AssignedTo = dto.AssignedUserId;
            entity.EstimationHours = dto.EstimationHours;
            entity.CompletedHours = dto.CompletedHours;
            entity.Status = dto.Status;

            await _db.SaveChangesAsync();

            return await (
                from t in _db.Tasks
                where t.TaskId == id
                join u in _db.Users on t.AssignedTo equals u.UserId into gj
                from u in gj.DefaultIfEmpty()
                select new TaskResponseDto
                {
                    Id = t.TaskId,
                    Title = t.Title ?? string.Empty,
                    Description = t.Description ?? string.Empty,
                    SprintItemId = t.SprintItemId,
                    AssignedUserId = t.AssignedTo,
                    AssignedUserName = u != null ? u.Username : null,
                    EstimationHours = t.EstimationHours,
                    CompletedHours = t.CompletedHours,
                    Status = t.Status ?? "To Do"
                }
            ).AsNoTracking().FirstAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Tasks.FirstOrDefaultAsync(t => t.TaskId == id);
            if (entity == null) return false;

            _db.Tasks.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignUserAsync(int taskId, int userId)
        {
            var entity = await _db.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
            if (entity == null) return false;

            var userExists = await _db.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists) return false;

            entity.AssignedTo = userId;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateHoursAsync(int id, decimal completedHours)
        {
            var entity = await _db.Tasks.FirstOrDefaultAsync(t => t.TaskId == id);
            if (entity == null) return false;

            entity.CompletedHours = completedHours;
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _db.Tasks.AnyAsync(t => t.TaskId == id);
        }
    }
}
