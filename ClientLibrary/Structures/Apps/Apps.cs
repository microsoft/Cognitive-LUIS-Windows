using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    public class Apps:LuisHandler
    {

        public Apps(string subscriptionKey, string baseApiUrl) : base(subscriptionKey, baseApiUrl) { }
              
        public async Task<LuisApplication> AddApplicationAsync(string name, string description = "", string culture = Culture.English)
        {
            var path = "apps";
            var uri = CreateHandlerUri(path);

            var app = new Application()
            {
                Name = name,
                Description = description,
                Culture = culture.ToString()
            };
            var responseMessage = await _httpClient.RestPost(uri, app);

            var response = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.IsSuccessStatusCode)
                return new LuisApplication(new Guid(response.Replace("\"", "")),_subscriptionKey);
            else
                throw new HttpRequestException(response);
        }

        public async Task<bool> DeleteApplicationAsync(string id)
        {
            return await DeleteApplicationAsync(new Guid(id));
        }

        public async Task<bool> DeleteApplicationAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            var path = $"apps/{id}";
            var uri = CreateHandlerUri(path);

            var responseMessage = await _httpClient.DeleteAsync(uri);

            var response = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.IsSuccessStatusCode)
                return true;
            else
                throw new HttpRequestException(response);
        }
    }
}
