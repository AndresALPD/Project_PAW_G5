using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.Sprints;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Services.Service
{
    public class SprintService : ISprintService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7250/api/sprints";

        public SprintService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<SprintDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<SprintDto>>(_baseUrl)
                   ?? new List<SprintDto>();
        }

        public async Task<SprintDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<SprintDto>($"{_baseUrl}/{id}");
        }

        public async Task<bool> CreateAsync(SprintCreateDto sprint)
        {
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, sprint);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, SprintCreateDto sprint)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", sprint);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}

