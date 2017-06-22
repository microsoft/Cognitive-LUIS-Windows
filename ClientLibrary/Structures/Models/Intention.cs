using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    public class Intention
    {
        private Guid _intentId;
        private string _intentName;

        public ExamplesHandler Examples { get; private set; }

        public Intention(string appId, string appKey, string subscriptionKey, string baseApiUrl, Guid intentId, string intentName)
        {
            _intentId = intentId;
            _intentName = intentName;

            Examples = new ExamplesHandler(appId, subscriptionKey, baseApiUrl, intentName);
        }
    }
}
