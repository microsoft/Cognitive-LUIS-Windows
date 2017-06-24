using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class ExamplesHandler : LuisHandler
    {
        private readonly string _intentName;

        public ExamplesHandler(string appId, string subscriptionKey, string baseApiUrl, string intentName) : base(appId, subscriptionKey, baseApiUrl)
        {
            if (string.IsNullOrEmpty(intentName)) throw new ArgumentNullException(nameof(intentName));
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

            var response = await _httpClient.RestPost(uri, label);
            return true;
        }

        public async Task<bool> AddLabelsAsync(IEnumerable<string> labels, string versionId = DEFAULT_VERSION_ID)
        {
            var labelsRequest = from label in labels
                                select new LabelRequest()
                                {
                                    IntentName = _intentName,
                                    Text = label
                                };
            return await AddLabelsAsync(labelsRequest, versionId);
        }

        private async Task<bool> AddLabelsAsync(IEnumerable<LabelRequest> labels, string versionId = DEFAULT_VERSION_ID)
        {
            if (labels == null || labels.Count() == 0) throw new ArgumentNullException(nameof(labels));

            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/examples");

            var response = await _httpClient.RestPost(uri, labels);
                return true;
        }
    }
}
