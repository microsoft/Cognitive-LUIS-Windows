using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    public class ExamplesHandler : LuisHandler
    {
        private readonly string _intentName;

        public ExamplesHandler(string appId, string subscriptionKey, string baseApiUrl, string intentName) : base(appId, subscriptionKey, baseApiUrl)
        {
            _intentName = intentName;
        }

        public async Task<bool> AddLabelAsync(string text, string versionId = DEFAULT_VERSION_ID)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));

            var label = new LabelRequest()
            {
                IntentName = _intentName,
                Text = text
            };

            return await AddLabelAsync(label, versionId);
        }

        private async Task<bool> AddLabelAsync(LabelRequest label, string versionId = DEFAULT_VERSION_ID)
        {
            if (label == null) throw new ArgumentNullException(nameof(label));

            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/example");

            var responseMessage = await _httpClient.RestPost(uri, label);

            var response = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.IsSuccessStatusCode)
                return true;
            else
                throw new HttpRequestException(response);
        }


        public async Task<bool> AddLabelsAsync(IEnumerable<string> texts, string versionId = DEFAULT_VERSION_ID)
        {
            var labels = from text in texts
                         select new LabelRequest()
                         {
                             IntentName = _intentName,
                             Text = text
                         };
            return await AddLabelsAsync(labels, versionId);
        }

        private async Task<bool> AddLabelsAsync(IEnumerable<LabelRequest> labels, string versionId = DEFAULT_VERSION_ID)
        {
            if (labels == null) throw new ArgumentNullException(nameof(labels));

            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/examples");

            var responseMessage = await _httpClient.RestPost(uri, labels);

            var response = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.IsSuccessStatusCode)
                return true;
            else
                throw new HttpRequestException(response);
        }
    }
}
