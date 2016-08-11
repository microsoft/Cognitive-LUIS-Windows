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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    /// <summary>
    /// Client for the LUIS REST API.
    /// </summary>
    public class LuisClient : IDisposable
    {
        /// <summary>
        /// flag to indidicate whether to return full result of all intents not just the top scoring intent (for preview features only)
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        /// flag to inidicate that preview features will be used
        /// </summary>
        public bool Preview { get; set; }

        protected string BASE_API_URL { get; set; }
        private readonly HttpClient _http;
        private readonly string _appId;

        /// <summary>
        /// Generates an API URI using the provided id and key for a registered LUIS application.
        /// </summary>
        /// <param name="id">Application id</param>
        /// <param name="subscriptionKey">Application key</param>
        /// <returns>Application URL for <see cref="LuisClient"/></returns>
        private string CreateApplicationUri(string id)
        {
            if (String.IsNullOrWhiteSpace(id)) throw new ArgumentException(nameof(id));

            return $"{BASE_API_URL}?id={id}&q=";
        }

        /// <summary>
        /// Generates an API URI (for preview features) using the provided id and key for a registered LUIS application.
        /// </summary>
        /// <param name="id">Application id</param>
        /// <param name="subscriptionKey">Application key</param>
        /// /// <param name="verbose">if true, return all intents not just the top scoring intent</param>
        /// <returns>Application Preview URL for <see cref="LuisClient"/></returns>
        private string CreateApplicationPreviewUri(string id)
        {
            if (String.IsNullOrWhiteSpace(id)) throw new ArgumentException(nameof(id));

            string verboseQuery = (Verbose) ? "&verbose=true" : "";

            return $"{BASE_API_URL}/preview?id={id}{verboseQuery}&q=";
        }

        /// <summary>
        ///Construct a new Luis client with a shared <see cref="HttpClient"/> instance.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application</param>
        /// <param name="appKey">The application subscription key of the LUIS application</param>
        /// <param name="preview">A flag indicating whether to use preview features or not (Dialogue)</param>
        /// top scoring in case of using the dialogue</param>
        public LuisClient(string appId, string appKey, bool preview = false)
        {
            if (String.IsNullOrWhiteSpace(appId)) throw new ArgumentException(nameof(appId));
            if (String.IsNullOrWhiteSpace(appKey)) throw new ArgumentException(nameof(appKey));

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("OCP-APIM-Subscription-Key", appKey);
            BASE_API_URL = "https://api.projectoxford.ai/luis/v1/application";

            Preview = preview;
            _appId = appId;
            _http = httpClient;
        }

        /// <summary>
        /// Encodes text to be suitable for http requests
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string EncodeRequest(string text)
        {
            string applicationUrl;
            if (Preview)
                applicationUrl = CreateApplicationPreviewUri(_appId);
            else
                applicationUrl = CreateApplicationUri(_appId);

            return applicationUrl + WebUtility.UrlEncode(text);
        }

        /// <summary>
        /// Replies to a question in the question in the dialog
        /// </summary>
        /// <param name="luisResult">The luis result obtained from the previous step (Predict/Reply)</param>
        /// <param name="text">The text to Reply with</param>
        /// <returns></returns>
        public async Task<LuisResult> Reply(LuisResult luisResult, string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return new LuisResult();

            if (luisResult == null) throw new ArgumentNullException("Luis result can't be null");
            if (luisResult.DialogResponse == null) throw new ArgumentNullException("Dialog can't be null in order to Reply");
            if (luisResult.DialogResponse.ContextId == null) throw new ArgumentNullException("Context id cannot be null");

            var uri = EncodeRequest(text) + $"&contextid={luisResult.DialogResponse.ContextId}";
            var result = await _http.RestGet(uri);
            return new LuisResult(this, result);
        }

        /// <summary>
        /// Sends a request to the LUIS service to parse <paramref name="text"/> for intents and entities.
        /// </summary>
        /// <param name="text">The text to Predict the intent for</param>
        /// <returns></returns>
        public async Task<LuisResult> Predict(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return new LuisResult();

            var uri = EncodeRequest(text);
            var result = await _http.RestGet(uri);
            return new LuisResult(this, result);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _http.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Disposes this <see cref="LuisClient"/> and associated managed objects.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
