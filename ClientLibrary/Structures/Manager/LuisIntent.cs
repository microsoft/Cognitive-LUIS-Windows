using System;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class LuisIntent
    {
        public ExamplesHandler Examples { get; private set; }

        public Guid Id { get; set; }
        public String Name { get; set; }


        public LuisIntent(string appId, string subscriptionKey, string baseApiUrl, Guid intentId, string intentName)
        {
            if (string.IsNullOrEmpty(appId)) throw new ArgumentNullException(nameof(appId));
            if (string.IsNullOrEmpty(subscriptionKey)) throw new ArgumentNullException(nameof(subscriptionKey));
            if (string.IsNullOrEmpty(baseApiUrl)) throw new ArgumentNullException(nameof(baseApiUrl));
            if (intentId == Guid.Empty) throw new ArgumentNullException(nameof(intentId));
            if (string.IsNullOrEmpty(intentName)) throw new ArgumentNullException(nameof(intentName));

            Id = intentId;
            Name = intentName;

            Examples = new ExamplesHandler(appId, subscriptionKey, baseApiUrl, intentName);
        }
    }
}
