using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    public class Apps
    {
        HttpClient _httpClient;

        public Apps(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private string CreateUri(string path)
        {
            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentException(nameof(path));
            return $"{_httpClient.BaseAddress.AbsoluteUri}/{path}";
        }

        public async Task<Guid> AddApplicationAsync(string name, string description = "", string culture = Culture.English)
        {
            var path = "apps";
            var uri = CreateUri(path);

            var app = new Application()
            {
                Name = name,
                Description = description,
                Culture = culture.ToString()
            };
            var responseMessage = await _httpClient.RestPost(uri, app);

            var response = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.IsSuccessStatusCode)
                return new Guid(response.Replace("\"", ""));
            else
                throw new HttpRequestException(response);
        }

        public async Task<bool> DeleteApplicationAsync(Guid id)
        {
            var path = $"apps/{id}";
            var uri = CreateUri(path);

            var responseMessage = await _httpClient.DeleteAsync(uri);

            var response = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.IsSuccessStatusCode)
                return true;
            else
                throw new HttpRequestException(response);
        }
    }
}
