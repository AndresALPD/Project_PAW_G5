using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Data.Context;
using Microsoft.EntityFrameworkCore;
using PAWScrum.Data.Context;
using PAWScrum.Models;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Models.Entities;


namespace PAWScrum.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly PAWScrumDbContext _context;

        public CommentRepository(PAWScrumDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetByTaskAsync(int taskId)
        {
            return await _context.Comments
                .AsNoTracking()
                .Where(c => c.TaskId == taskId)     // ✅ columna real
                .Include(c => c.User)               // opcional, útil para mostrar nombre
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.Comments
                .AsNoTracking()
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return false;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}