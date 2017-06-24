using System;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class SettingsHandler : LuisHandler
    {
        public SettingsHandler(string appId, string subscriptionKey, string baseApiUrl) : base(appId, subscriptionKey, baseApiUrl) { }

        public async Task AssignAppKey(string appKey, string versionId = DEFAULT_VERSION_ID)
        {
            if (string.IsNullOrEmpty(appKey)) throw new ArgumentNullException(nameof(appKey));

            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/assignedkey");
            await _httpClient.RestPut(uri, appKey);
        }

        public async Task PublishAsync(string versionId = DEFAULT_VERSION_ID, bool isStaging = false)
        {
            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/publish");
            var request = new
            {
                VersionId = versionId,
                IsStaging = isStaging
            };

            var response = await _httpClient.RestPost(uri, request);
        }
    }
}
