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

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class LuisIntent
    {
        /// <summary>
        /// Handles examples operations.
        /// </summary>
        public ExamplesHandler Examples { get; private set; }
        /// <summary>
        /// Intent Id.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Intent name.
        /// </summary>
        public String Name { get; set; }


        /// <summary>
        /// Construct a new Luis intent.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application.</param>
        /// <param name="subscriptionKey">The subscription key of the LUIS account.</param>
        /// <param name="baseApiUrl">Root URI for the service endpoint.</param>
        /// <param name="intentId">The intent ID of the LUIS application.</param>
        /// <param name="intentName">The intent name of the LUIS application.</param>
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
