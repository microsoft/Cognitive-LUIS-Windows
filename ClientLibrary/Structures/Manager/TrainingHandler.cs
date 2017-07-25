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
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class TrainingHandler : LuisHandler
    {
        private const int TRAINING_STATUS_DELAY = 1000;

        /// <summary>
        /// Creates a training LUIS service handler.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application.</param>
        /// <param name="subscriptionKey">The subscription key of the LUIS account.</param>
        /// <param name="baseApiUrl">Root URI for the service endpoint.</param>
        public TrainingHandler(string appId, string subscriptionKey, string baseApiUrl) : base(appId, subscriptionKey, baseApiUrl) { }


        /// <summary>
        /// Starts the training of a LUIS application.
        /// </summary>
        /// <param name="versionId">Application version to be used.</param>
        /// <returns></returns>
        public async Task TrainAsync(string versionId = DEFAULT_VERSION_ID)
        {
            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/train");
            var response = await _httpClient.RestPost(uri);
        }

        /// <summary>
        /// Starts the training of a LUIS application.
        /// </summary>
        /// <param name="token"><seealso cref="CancellationToken"/> used to control the training's execution flow.</param>
        /// <param name="versionId">Application version to be used.</param>
        /// <returns></returns>
        public async Task TrainAsync(CancellationToken token, string versionId = DEFAULT_VERSION_ID)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/train");
            var response = await _httpClient.RestPost(uri);

            while (true)
            {
                if (token.IsCancellationRequested)
                    return;

                var status = await GetTrainingStatusAsync(versionId);
                if (status == "Fail")
                    throw new Exception("Training has failed.");
                if (status == "Success" || status == "UpToDate")
                    break;

               await Task.Delay(TRAINING_STATUS_DELAY);
            }
        }

        /// <summary>
        /// Gets the training status of a LUIS application.
        /// </summary>
        /// <param name="versionId">Application version to be used.</param>
        /// <returns>Training status.</returns>
        public async Task<string> GetTrainingStatusAsync(string versionId = DEFAULT_VERSION_ID)
        {
            var uri = CreateHandlerUri($"api/v2.0/apps/{_appId}/versions/{versionId}/train");
            var response = await _httpClient.RestGet(uri);
            return response[0]["details"]["status"].ToString();
        }

    }
}
