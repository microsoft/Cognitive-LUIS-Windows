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

using Microsoft.Cognitive.LUIS.Manager;
using System;

namespace Microsoft.Cognitive.LUIS
{
    /// <summary>
    /// Client for managing LUIS applications through its REST API.
    /// </summary>
    public class LuisManager
    {
        private const string DEFAULT_DOMAIN = "westus";
        private const string DEFAULT_BASE_URI = "https://{0}.api.cognitive.microsoft.com/luis/api/v2.0";

        protected string BASE_API_URL { get; set; }

        /// <summary>
        /// Handles applications operations.
        /// </summary>
        public ApplicationHandler Apps { get; private set; }

        /// <summary>
        /// Construct a new LUIS subscription manager based on a Subscription Key.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key of the LUIS account.</param>
        /// <param name="domain">String to represent the domain of the endpoint.</param>
        /// <param name="baseUri">Root URI for the service endpoint.</param>
        public LuisManager(string subscriptionKey, string domain = DEFAULT_DOMAIN, string baseUri = DEFAULT_BASE_URI)
        {
            if (string.IsNullOrEmpty(subscriptionKey)) throw new ArgumentNullException(nameof(subscriptionKey));

            BASE_API_URL = string.Format(baseUri, domain);

            Apps = new ApplicationHandler(subscriptionKey, BASE_API_URL);
        }

    }
}
