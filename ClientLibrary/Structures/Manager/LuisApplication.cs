//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
//
// Microsoft Cognitive Services (formerly Project Oxford): https://www.microsoft.com/cognitive-services

//
// Microsoft Cognitive Services (formerly Project Oxford) GitHub:
// https://github.com/Microsoft/ProjectOxford-ClientSDK

//
// Copyright (c) Microsoft Corporation
// All rights reserved.
//
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Diagnostics;

namespace Microsoft.Cognitive.LUIS.Manager
{
    [DebuggerDisplay("Name: {Name}, Id: {Id}")]
    public class LuisApplication
    {
        private const string DEFAULT_DOMAIN = "westus";
        private const string DEFAULT_BASE_URI = "https://{0}.api.cognitive.microsoft.com/luis";

        protected string BASE_API_URL { get; set; }

        /// <summary>
        /// Handles intents operations.
        /// </summary>
        public IntentHandler Intent { get; private set; }
        /// <summary>
        /// Handles settings operations.
        /// </summary>
        public SettingsHandler Settings { get; private set; }
        /// <summary>
        /// Handles training operations.
        /// </summary>
        public TrainingHandler Training { get; private set; }

        public string Id { get; private set; }
        public string Name { get; private set; }

        /// <summary>
        /// Construct a new Luis application.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application.</param>
        /// <param name="appName">The application Name of the LUIS application.</param>
        /// <param name="subscriptionKey">The subscription key of the LUIS account.</param>
        /// <param name="domain">String to represent the domain of the endpoint.</param>
        public LuisApplication(string appId, string appName, string subscriptionKey, string domain = DEFAULT_DOMAIN) : this(appId, appName, subscriptionKey, DEFAULT_BASE_URI, domain) { }

        /// <summary>
        /// Construct a new Luis application.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application.</param>
        /// <param name="appName">The application Name of the LUIS application.</param>
        /// <param name="subscriptionKey">The subscription key of the LUIS account.</param>
        /// <param name="domain">String to represent the domain of the endpoint.</param>
        public LuisApplication(Guid appId, string appName, string subscriptionKey, string domain = DEFAULT_DOMAIN) : this(appId.ToString(), appName, subscriptionKey, DEFAULT_BASE_URI, domain) { }

        /// <summary>
        /// Construct a new Luis application.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application.</param>
        /// <param name="appName">The application Name of the LUIS application.</param>
        /// <param name="subscriptionKey">The subscription key of the LUIS account.</param>
        /// <param name="baseApiUrl">Root URI for the service endpoint.</param>
        /// <param name="domain">String to represent the domain of the endpoint.</param>
        public LuisApplication(string appId, string appName, string subscriptionKey, string baseApiUrl, string domain = DEFAULT_DOMAIN)
        {
            if (string.IsNullOrWhiteSpace(appId)) throw new ArgumentException(nameof(appId));
            if (string.IsNullOrWhiteSpace(appName)) throw new ArgumentException(nameof(appName));
            if (string.IsNullOrWhiteSpace(subscriptionKey)) throw new ArgumentNullException(nameof(subscriptionKey));
            if (string.IsNullOrWhiteSpace(baseApiUrl)) throw new ArgumentException(nameof(baseApiUrl));

            BASE_API_URL = string.Format(baseApiUrl, domain);

            Id = appId;
            Name = appName;

            Intent = new IntentHandler(appId, subscriptionKey, BASE_API_URL);
            Settings = new SettingsHandler(appId, subscriptionKey, BASE_API_URL);
            Training = new TrainingHandler(appId, subscriptionKey, BASE_API_URL);
        }
    }
}
