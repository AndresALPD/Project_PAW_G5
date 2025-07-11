using PAWScrum.Architecture.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace PAWScrum.Architecture.Providers
{

    public class RestProvider : IRestProvider
    {
        private readonly HttpClient _client;

        public RestProvider(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> PostAsync(string url, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
