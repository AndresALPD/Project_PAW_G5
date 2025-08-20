using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.SprintBacklog;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Services.Service
{
    public class SprintBacklogService : ISprintBacklogService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7250/api/sprintbacklog";

        public SprintBacklogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<SprintBacklogDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<SprintBacklogDto>>(BaseUrl)
                   ?? new List<SprintBacklogDto>();
        }

        public async Task<SprintBacklogDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<SprintBacklogDto>($"{BaseUrl}/{id}");
        }

        public async Task<bool> CreateAsync(SprintBacklogCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, SprintBacklogCreateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
