using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    public class LuisApplication
    {
        private const string DEFAULT_DOMAIN = "westus";
        private const string DEFAULT_BASE_URI = "https://{0}.api.cognitive.microsoft.com/luis";

        protected string BASE_API_URL { get; set; }
        private readonly bool _verbose;

        public IntentionsHandler Intentions { get; private set; }
        public SettingsHandler Settings { get; private set; }
        public TrainingHandler Training { get; private set; }

        public string Id { get; private set; }

        /// <summary>
        /// Construct a new Luis client with a shared <see cref="HttpClient"/> instance.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application</param>
        /// <param name="appKey">The application subscription key of the LUIS application</param>
        /// <param name="verbose">A flag indicating whether to use verbose version or not</param>
        /// <param name="domain">String to represent the domain of the endpoint</param>
        /// top scoring in case of using the dialogue</param>
        public LuisApplication(string appId, string subscriptionKey, bool verbose = true, string domain = DEFAULT_DOMAIN) : this(appId, subscriptionKey, DEFAULT_BASE_URI, verbose, domain) { }

        public LuisApplication(Guid appId, string subscriptionKey, bool verbose = true, string domain = DEFAULT_DOMAIN) : this(appId.ToString(), subscriptionKey, DEFAULT_BASE_URI, verbose, domain) { }

        /// <summary>
        /// Construct a new Luis client with a shared <see cref="HttpClient"/> instance.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application</param>
        /// <param name="appKey">The application subscription key of the LUIS application</param>
        /// <param name="baseApiUrl">Root URI for the service endpoint.</param>
        /// <param name="verbose">A flag indicating whether to use verbose version or not</param>
        /// top scoring in case of using the dialogue</param>
        public LuisApplication(string appId, string subscriptionKey, string baseApiUrl, bool verbose = true, string domain = DEFAULT_DOMAIN)
        {
            if (string.IsNullOrWhiteSpace(appId)) throw new ArgumentException(nameof(appId));
            if (string.IsNullOrEmpty(subscriptionKey)) throw new ArgumentNullException(nameof(subscriptionKey));
            if (string.IsNullOrWhiteSpace(baseApiUrl)) throw new ArgumentException(nameof(baseApiUrl));

            BASE_API_URL = string.Format(baseApiUrl, domain);

            Id = appId;
            _verbose = verbose;

            Intentions = new IntentionsHandler(appId, subscriptionKey, BASE_API_URL);
            Settings = new SettingsHandler(appId, subscriptionKey, BASE_API_URL);
            Training = new TrainingHandler(appId, subscriptionKey, BASE_API_URL);
        }
    }
}
