using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class IntentHandler : LuisHandler
    {
        public IntentHandler(string appId, string subscriptionKey, string baseApiUrl) : base(appId, subscriptionKey, baseApiUrl) { }

        public async Task<LuisIntent> AddIntentionAsync(string name, string versionId = DEFAULT_VERSION_ID)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/intents");

            var intention = new
            {
                Name = name
            };
            var response = await _httpClient.RestPost(uri, intention);
            var id = new Guid(response.ToString());

            return new LuisIntent(_appId, _subscriptionKey, _baseApiUrl, id, name);
        }

        public async Task<LuisIntent> GetIntentionAsync(string name, string versionId = DEFAULT_VERSION_ID)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var path = $"api/v2.0/apps/{_appId}/versions/{versionId}/intents";
            var uri = CreateHandlerUri(path);

            var response = await _httpClient.RestGet(uri);
            var intents = response.ToObject<IEnumerable<Intent>>();

            var intent = (from i in intents
                       where i.Name.ToLower() == name.ToLower()
                       select new LuisIntent(_appId, _subscriptionKey, _baseApiUrl, i.Id, name)).First();

            return intent;
        }

        public async Task<LuisIntent> GetIntentionAsync(Guid id, string versionId = DEFAULT_VERSION_ID)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            var path = $"api/v2.0/apps/{_appId}/versions/{versionId}/intents/{id}";
            var uri = CreateHandlerUri(path);

            var response = await _httpClient.RestGet(uri);
            var intent = response.ToObject<Intent>();

            return new LuisIntent(_appId, _subscriptionKey, _baseApiUrl, intent.Id, intent.Name);
        }
    }
}
