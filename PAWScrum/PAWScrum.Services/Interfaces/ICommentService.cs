using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Models.DTOs.Comments;

namespace PAWScrum.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentResponseDto>> GetByTaskAsync(int taskId);
        Task<CommentResponseDto> GetByIdAsync(int id);
        Task<CommentResponseDto> CreateAsync(CommentCreateDto dto);
        Task<CommentResponseDto> UpdateAsync(int id, CommentCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
