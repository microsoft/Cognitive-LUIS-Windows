using Microsoft.Cognitive.LUIS.Manager;
using System;

namespace Microsoft.Cognitive.LUIS
{
    public class LuisManager
    {
        private const string DEFAULT_DOMAIN = "westus";
        private const string DEFAULT_BASE_URI = "https://{0}.api.cognitive.microsoft.com/luis/api/v2.0";

        protected string BASE_API_URL { get; set; }

        public ApplicationHandler Apps { get; private set; }

        public LuisManager(string subscriptionKey, string domain = DEFAULT_DOMAIN, string baseUri = DEFAULT_BASE_URI)
        {
            if (string.IsNullOrEmpty(subscriptionKey)) throw new ArgumentNullException(nameof(subscriptionKey));

            BASE_API_URL = string.Format(baseUri, domain);

            Apps = new ApplicationHandler(subscriptionKey, BASE_API_URL);
        }

    }
}
