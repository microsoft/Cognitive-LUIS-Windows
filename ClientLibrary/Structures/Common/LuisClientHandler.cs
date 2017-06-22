using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    public abstract class LuisHandler
    {
        protected const string DEFAULT_VERSION_ID = "0.1";

        protected string _appId;
        protected string _appKey;
        protected string _subscriptionKey;
        protected string _baseApiUrl;

        protected HttpClient _httpClient;

        public LuisHandler(string subscriptionKey, string baseApiUrl) : this(string.Empty, subscriptionKey, baseApiUrl) { }
        public LuisHandler(string appId, string subscriptionKey, string baseApiUrl)
        {
            _appId = appId;
            _subscriptionKey = subscriptionKey;
            _baseApiUrl = baseApiUrl;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("OCP-APIM-Subscription-Key", subscriptionKey);
        }

        protected string CreateHandlerUri(string path)
        {
            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentException(nameof(path));
            return $"{_baseApiUrl}/{path}";
        }
    }
}
