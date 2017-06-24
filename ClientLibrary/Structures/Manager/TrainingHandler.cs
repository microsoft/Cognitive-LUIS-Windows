using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class TrainingHandler : LuisHandler
    {
        public TrainingHandler(string appId, string subscriptionKey, string baseApiUrl) : base(appId, subscriptionKey, baseApiUrl) { }

        public async Task TrainAsync(string versionId = DEFAULT_VERSION_ID)
        {
            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/train");
            var response = await _httpClient.RestPost(uri);
        }

        public async Task<string> GetTrainingStatusAsync(string versionId = DEFAULT_VERSION_ID)
        {
            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/train");
            var response = await _httpClient.RestGet(uri);
            return response[0]["details"]["status"].ToString();
        }

    }
}
