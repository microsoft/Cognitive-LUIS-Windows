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
using System.Net.Http;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public abstract class LuisHandler
    {
        protected const string DEFAULT_VERSION_ID = "0.1";

        protected string _appId;
        protected string _subscriptionKey;
        protected string _baseApiUrl;

        protected HttpClient _httpClient;

        /// <summary>
        /// Creates a generic LUIS service handler.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key of the LUIS account.</param>
        /// <param name="baseApiUrl">Root URI for the service endpoint.</param>
        public LuisHandler(string subscriptionKey, string baseApiUrl) : this(string.Empty, subscriptionKey, baseApiUrl) { }

        /// <summary>
        /// Creates a generic LUIS service handler.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application.</param>
        /// <param name="subscriptionKey">The subscription key of the LUIS account.</param>
        /// <param name="baseApiUrl">Root URI for the service endpoint.</param>
        public LuisHandler(string appId, string subscriptionKey, string baseApiUrl)
        {
            if (string.IsNullOrEmpty(subscriptionKey)) throw new ArgumentNullException(nameof(subscriptionKey));
            if (string.IsNullOrEmpty(baseApiUrl)) throw new ArgumentNullException(nameof(baseApiUrl));

            _appId = appId;
            _subscriptionKey = subscriptionKey;
            _baseApiUrl = baseApiUrl;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("OCP-APIM-Subscription-Key", subscriptionKey);
        }

        /// <summary>
        /// Creates the service URL using the Base REST API URL and a specific method.
        /// </summary>
        /// <param name="path">Path URL to the specific method inside LUIS REST API.</param>
        /// <returns>Service URL.</returns>
        protected string CreateHandlerUri(string path)
        {
            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentException(nameof(path));
            return $"{_baseApiUrl}/{path}";
        }
    }
}
