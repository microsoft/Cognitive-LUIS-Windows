using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class ApplicationHandler : LuisHandler
    {

        public ApplicationHandler(string subscriptionKey, string baseApiUrl) : base(subscriptionKey, baseApiUrl) { }

        public async Task<LuisApplication> CreateApplicationAsync(string name, string description = "", string culture = Culture.English)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var path = "apps";
            var uri = CreateHandlerUri(path);

            var app = new Application()
            {
                Name = name,
                Description = description,
                Culture = culture.ToString()
            };
            var response = await _httpClient.RestPost(uri, app);
            var id = new Guid(response.ToString());
            return new LuisApplication(id, name, _subscriptionKey);
        }

        public async Task<LuisApplication> GetApplicationAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var path = "apps";
            var uri = CreateHandlerUri(path);

            var response = await _httpClient.RestGet(uri);
            var applications = response.ToObject<IEnumerable<Application>>();

            var app = (from a in applications
                       where a.Name.ToLower() == name.ToLower()
                       select new LuisApplication(a.Id, name, _subscriptionKey)).First();

            return app;
        }

        public async Task<LuisApplication> GetApplicationAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            var path = $"apps/{id.ToString()}";
            var uri = CreateHandlerUri(path);

            var response = await _httpClient.RestGet(uri);
            var app = response.ToObject<Application>();

            return new LuisApplication(app.Id, app.Name, _subscriptionKey);
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
