using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    public class LuisManager
    {
        private const string DEFAULT_DOMAIN = "westus";
        private const string DEFAULT_BASE_URI = "https://{0}.api.cognitive.microsoft.com/luis/api/v2.0";

        private HttpClient _httpClient;
       
        public LuisManager(string programmaticAPIKey, string domain = DEFAULT_DOMAIN, string baseUri = DEFAULT_BASE_URI)
        {
            if (string.IsNullOrEmpty(programmaticAPIKey)) throw new ArgumentNullException(nameof(programmaticAPIKey));

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("OCP-APIM-Subscription-Key", programmaticAPIKey);
            _httpClient.BaseAddress = new Uri(string.Format(baseUri, domain));

            Apps = new Apps(_httpClient);
        }

        public Apps Apps { get; private set; }
    }
}
