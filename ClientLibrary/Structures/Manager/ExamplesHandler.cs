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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class ExamplesHandler : LuisHandler
    {
        private readonly string _intentName;

        /// <summary>
        /// Creates a examples LUIS service handler.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application.</param>
        /// <param name="subscriptionKey">The subscription key of the LUIS account.</param>
        /// <param name="baseApiUrl">Root URI for the service endpoint.</param>
        /// <param name="intentName">The parent intent name.</param>
        public ExamplesHandler(string appId, string subscriptionKey, string baseApiUrl, string intentName) : base(appId, subscriptionKey, baseApiUrl)
        {
            if (string.IsNullOrEmpty(intentName)) throw new ArgumentNullException(nameof(intentName));
            _intentName = intentName;
        }

        /// <summary>
        /// Adds a text label to an LUIS intent.
        /// </summary>
        /// <param name="text">Text of the example.</param>
        /// <param name="versionId">Application version to be used.</param>
        /// <returns>Operation success status.</returns>
        public async Task<bool> AddLabelAsync(string text, string versionId = DEFAULT_VERSION_ID)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentNullException(nameof(text));

            var label = new LabelRequest()
            {
                IntentName = _intentName,
                Text = text
            };

            return await AddLabelAsync(label, versionId);
        }

        /// <summary>
        /// Adds a text label to an LUIS intent.
        /// </summary>
        /// <param name="label">Internal request structure for label example.</param>
        /// <param name="versionId">Application version to be used.</param>
        /// <returns>Operation success status.</returns>
        private async Task<bool> AddLabelAsync(LabelRequest label, string versionId = DEFAULT_VERSION_ID)
        {
            if (label == null) throw new ArgumentNullException(nameof(label));

            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/example");

            var response = await _httpClient.RestPost(uri, label);
            return true;
        }

        /// <summary>
        /// Adds text labels to an LUIS intent.
        /// </summary>
        /// <param name="labels">List of examples.</param>
        /// <param name="versionId">Application version to be used.</param>
        /// <returns>Operation success status.</returns>
        public async Task<bool> AddLabelsAsync(IEnumerable<string> labels, string versionId = DEFAULT_VERSION_ID)
        {
            var labelsRequest = from label in labels
                                select new LabelRequest()
                                {
                                    IntentName = _intentName,
                                    Text = label
                                };
            return await AddLabelsAsync(labelsRequest, versionId);
        }

        /// <summary>
        /// Adds text labels to an LUIS intent.
        /// </summary>
        /// <param name="labels">Internal request structure for label examples.</param>
        /// <param name="versionId">Application version to be used.</param>
        /// <returns>Operation success status.</returns>
        private async Task<bool> AddLabelsAsync(IEnumerable<LabelRequest> labels, string versionId = DEFAULT_VERSION_ID)
        {
            if (labels == null || labels.Count() == 0) throw new ArgumentNullException(nameof(labels));

            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/examples");

            var response = await _httpClient.RestPost(uri, labels);
                return true;
        }
    }
}
