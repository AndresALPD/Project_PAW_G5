using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using PAWScrum.Business.Interfaces;
using PAWScrum.Models;
using PAWScrum.Models.DTOs.ProductBacklog;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.Services.Service
{
    public class ProductBacklogService : IProductBacklogService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7250/api/productbacklog";

        public ProductBacklogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductBacklogDto>> GetAllAsync() =>
            await _httpClient.GetFromJsonAsync<IEnumerable<ProductBacklogDto>>(BaseUrl)
            ?? new List<ProductBacklogDto>();

        public async Task<ProductBacklogDto?> GetByIdAsync(int id) =>
            await _httpClient.GetFromJsonAsync<ProductBacklogDto>($"{BaseUrl}/{id}");

        public async Task<bool> CreateAsync(ProductBacklogCreateDto dto)
        {
            var res = await _httpClient.PostAsJsonAsync(BaseUrl, dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, ProductBacklogCreateDto dto)
        {
            var res = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var res = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return res.IsSuccessStatusCode;
        }
    }
}

