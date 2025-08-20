using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Data;                 // PAWScrumDbContext
using PAWScrum.Data.Context;
using PAWScrum.Models.Entities;      // Comment
using PAWScrum.Repositories.Interfaces;

namespace PAWScrum.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
     {
            private readonly PAWScrumDbContext _ctx;
            public CommentRepository(PAWScrumDbContext ctx) => _ctx = ctx;

            public async Task<IEnumerable<Comment>> GetByTaskAsync(int taskId)
            {
                return await _ctx.Comments
                    .AsNoTracking()
                    .Where(c => c.TaskId == taskId)
                    .Include(c => c.User)                      // opcional
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();
            }

            public async Task<Comment?> GetByIdAsync(int id)
            {
                return await _ctx.Comments
                    .AsNoTracking()
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.CommentId == id);
            }

            public async Task<Comment> AddAsync(Comment comment)
            {
                _ctx.Entry(comment).State = EntityState.Added;
                await _ctx.SaveChangesAsync();
                return comment;
            }

            public async Task<Comment> UpdateAsync(Comment comment)
            {
                _ctx.Entry(comment).State = EntityState.Modified;
                await _ctx.SaveChangesAsync();
                return comment;
            }

            public async Task<bool> DeleteAsync(int id)
            {
                var entity = await _ctx.Comments.FindAsync(id);
                if (entity is null) return false;

                _ctx.Comments.Remove(entity);
                await _ctx.SaveChangesAsync();
                return true;
            }
        }
    }
