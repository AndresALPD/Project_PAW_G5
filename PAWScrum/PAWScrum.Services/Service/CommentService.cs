using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.Comments;
using PAWScrum.Repositories.Interfaces;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Services.Service
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentResponseDto>> GetByTaskAsync(int taskId)
        {
            var comments = await _repository.GetByTaskAsync(taskId);
            return _mapper.Map<IEnumerable<CommentResponseDto>>(comments);
        }

        public async Task<CommentResponseDto> GetByIdAsync(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            return _mapper.Map<CommentResponseDto>(comment);
        }

        public async Task<CommentResponseDto> CreateAsync(CommentCreateDto dto)
        {
            var comment = _mapper.Map<Comment>(dto);
            comment.CreatedAt = DateTime.UtcNow;
            await _repository.AddAsync(comment);
            return _mapper.Map<CommentResponseDto>(comment);
        }

        public async Task<CommentResponseDto> UpdateAsync(int id, CommentCreateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
            return _mapper.Map<CommentResponseDto>(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
