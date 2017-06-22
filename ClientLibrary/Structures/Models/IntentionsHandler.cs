using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    public class IntentionsHandler : LuisHandler
    {
        public IntentionsHandler(string appId, string subscriptionKey, string baseApiUrl):base(appId, subscriptionKey, baseApiUrl)
        {
        }

        public async Task<Intention> AddIntentionAsync(string name, string versionId = DEFAULT_VERSION_ID)
        {
            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/intents");

            var intention = new
            {
                Name = name
            };
            var responseMessage = await _httpClient.RestPost(uri, intention);

            var response = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.IsSuccessStatusCode)
                return new Intention(_appId, _appKey, _subscriptionKey, _baseApiUrl, new Guid(response.Replace("\"", "")), name);
            else
                throw new HttpRequestException(response);
        }

        public async Task<Intention> GetIntention(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            throw new NotImplementedException();
        }

        public async Task<Intention> GetIntention(Guid intentionId)
        {
            if (intentionId == Guid.Empty) throw new ArgumentNullException(nameof(intentionId));

            throw new NotImplementedException();
        }


    }
}
