using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.Comments;
using PAWScrum.Models.Entities;
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
            var items = await _repository.GetByTaskAsync(taskId);
            return items.Select(c => _mapper.Map<CommentResponseDto>(c));
        }

        public async Task<CommentResponseDto> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<CommentResponseDto>(item);
        }

        public async Task<CommentResponseDto> CreateAsync(CommentCreateDto dto)
        {
            var entity = _mapper.Map<Comment>(dto);
            var saved = await _repository.AddAsync(entity);
            return _mapper.Map<CommentResponseDto>(saved);
        }

        public async Task<CommentResponseDto> UpdateAsync(int id, CommentCreateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<CommentResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
            => await _repository.DeleteAsync(id);
    }
}