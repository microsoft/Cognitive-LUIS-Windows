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
using System.Linq;
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
        private const string DEFAULT_DOMAIN = "westus";
        private const string DEFAULT_BASE_URI = "https://{0}.api.cognitive.microsoft.com/luis/v2.0/apps";

        protected string BASE_API_URL { get; set; }
        private readonly HttpClient _http;
        private readonly string _appId;
        private readonly bool _verbose;
        private readonly bool? _spellCheckOverride;

        /// <summary>
        /// Generates an API URI using the provided id and key for a registered LUIS application.
        /// </summary>
        /// <param name="id">Application id</param>
        /// <param name="subscriptionKey">Application key</param>
        /// <returns>Application URL for <see cref="LuisClient"/></returns>
        private string CreateApplicationUri(string id)
        {
            if (String.IsNullOrWhiteSpace(id)) throw new ArgumentException(nameof(id));

            string queryString = string.Join("&",
                new[]
                {
                    _verbose ? "verbose=true" : null,
                    _spellCheckOverride.HasValue ? $"spellCheck={_spellCheckOverride.ToString().ToLower()}" : null,
                    "q="
                }.Where(s => s != null));

            return $"{BASE_API_URL}/{id}?{queryString}";
        }

        /// <summary>
        /// Construct a new Luis client with a shared <see cref="HttpClient"/> instance.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application</param>
        /// <param name="appKey">The application subscription key of the LUIS application</param>
        /// <param name="verbose">A flag indicating whether to use verbose version or not</param>
        /// <param name="domain">String to represent the domain of the endpoint</param>
        /// <param name="spellCheckOverride">True or False to override the default Luis spellCheck behavior</param>
        public LuisClient(string appId, string appKey, bool verbose = true, string domain = DEFAULT_DOMAIN, bool? spellCheckOverride = null) : this(appId, appKey, DEFAULT_BASE_URI, verbose, domain, spellCheckOverride) { }

        /// <summary>
        /// Construct a new Luis client with a shared <see cref="HttpClient"/> instance.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application</param>
        /// <param name="appKey">The application subscription key of the LUIS application</param>
        /// <param name="baseApiUrl">Root URI for the service endpoint.</param>
        /// <param name="verbose">A flag indicating whether to use verbose version or not</param>
        /// <param name="spellCheckOverride">True or False to override the default Luis spellCheck behavior</param>
        public LuisClient(string appId, string appKey, string baseApiUrl, bool verbose = true, string domain = DEFAULT_DOMAIN, bool? spellCheckOverride = null)
        {
            if (String.IsNullOrWhiteSpace(appId)) throw new ArgumentException(nameof(appId));
            if (String.IsNullOrWhiteSpace(appKey)) throw new ArgumentException(nameof(appKey));
            if (String.IsNullOrWhiteSpace(baseApiUrl)) throw new ArgumentException(nameof(baseApiUrl));

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("OCP-APIM-Subscription-Key", appKey);
            BASE_API_URL = string.Format(baseApiUrl, domain);

            _appId = appId;
            _http = httpClient;
            _verbose = verbose;
            _spellCheckOverride = spellCheckOverride;
        }

        /// <summary>
        /// Encodes text to be suitable for http requests
        /// </summary>
        /// <param name="text"></param>
        /// <param name="forceSetParameterName">The name of a parameter to reset in the current dialog</param>
        /// <returns></returns>
        private string EncodeRequest(string text, string forceSetParameterName = null)
        {
            string applicationUrl;
            applicationUrl = CreateApplicationUri(_appId);
            applicationUrl += WebUtility.UrlEncode(text);
            if (!String.IsNullOrWhiteSpace(forceSetParameterName))
            {
                applicationUrl += $"&forceset={forceSetParameterName}";
            }
            return applicationUrl;
        }

        /// <summary>
        /// Replies to a question in the question in the dialog
        /// </summary>
        /// <param name="luisResult">The luis result obtained from the previous step (Predict/Reply)</param>
        /// <param name="text">The text to Reply with</param>
        /// <param name="forceSetParameterName">The name of a parameter to reset in the current dialog</param>
        /// <returns></returns>
        public async Task<LuisResult> Reply(LuisResult luisResult, string text, string forceSetParameterName = null)
        {
            if (String.IsNullOrWhiteSpace(text)) throw new ArgumentException("Invalid text query");

            if (luisResult == null) throw new ArgumentNullException("Luis result can't be null");
            if (luisResult.DialogResponse == null) throw new ArgumentNullException("Dialog can't be null in order to Reply");
            if (luisResult.DialogResponse.ContextId == null) throw new ArgumentNullException("Context id cannot be null");

            var uri = EncodeRequest(text, forceSetParameterName) + $"&contextid={luisResult.DialogResponse.ContextId}";
            var result = await _http.RestGet(uri).ConfigureAwait(false);
            return new LuisResult(this, result);
        }

        /// <summary>
        /// Sends a request to the LUIS service to parse <paramref name="text"/> for intents and entities.
        /// </summary>
        /// <param name="text">The text to Predict the intent for</param>
        /// <returns></returns>
        public async Task<LuisResult> Predict(string text)
        {
            if (String.IsNullOrWhiteSpace(text)) throw new ArgumentException("Invalid text query");

            var uri = EncodeRequest(text);
            var result = await _http.RestGet(uri).ConfigureAwait(false);
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
