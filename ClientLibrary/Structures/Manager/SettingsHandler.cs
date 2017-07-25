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
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class SettingsHandler : LuisHandler
    {
        /// <summary>
        /// Creates a settings LUIS service handler.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application.</param>
        /// <param name="subscriptionKey">The subscription key of the LUIS account.</param>
        /// <param name="baseApiUrl">Root URI for the service endpoint.</param>
        public SettingsHandler(string appId, string subscriptionKey, string baseApiUrl) : base(appId, subscriptionKey, baseApiUrl) { }

        /// <summary>
        /// Add a key to an LUIS application.
        /// </summary>
        /// <param name="appKey">Application key.</param>
        /// <param name="versionId">Application version to be used.</param>
        /// <returns></returns>
        public async Task AssignAppKey(string appKey, string versionId = DEFAULT_VERSION_ID)
        {
            if (string.IsNullOrEmpty(appKey)) throw new ArgumentNullException(nameof(appKey));

            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/assignedkey");
            await _httpClient.RestPut(uri, appKey);
        }

        /// <summary>
        /// Publish a LUIS application.
        /// </summary>
        /// <param name="versionId">Application version to be used.</param>
        /// <param name="isStaging">Flag signaling if the application should be published on staging or not.</param>
        /// <returns></returns>
        public async Task PublishAsync(string versionId = DEFAULT_VERSION_ID, bool isStaging = false)
        {
            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/publish");
            var request = new
            {
                VersionId = versionId,
                IsStaging = isStaging
            };

            var response = await _httpClient.RestPost(uri, request);
        }
    }
}
