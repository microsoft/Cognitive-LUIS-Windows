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


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    /// <summary>
    /// Utility extensions to streamline working with a REST API.
    /// </summary>
    public static class HttpClientRestExtensions
    {

        /// <summary>
        /// Sends a GET request to the server and parses the response a JSON.
        /// </summary>
        /// <param name="client"><seealso cref="HttpClient"/> to send the request using it.</param>
        /// <param name="url">URL of the API to make the request to.</param>
        /// <returns>JObject containing the parsed response.</returns>
        public async static Task<JToken> RestGet(this HttpClient client, string url)
        {
            return await client.RestCall(HttpMethod.Get, url);
        }

        /// <summary>
        /// Sends a PUT request to the server and parses the response a JSON.
        /// </summary>
        /// <param name="client"><seealso cref="HttpClient"/> to send the request using it.</param>
        /// <param name="url">URL of the API to make the request to.</param>
        /// <param name="body">Optional body request.</param>
        /// <returns>JObject containing the parsed response.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException">Thrown if a non-success result is returned from the server.</exception>
        public async static Task<JToken> RestPut(this HttpClient client, string url, object body = null)
        {
            return await client.RestCall(HttpMethod.Put, url, body);
        }

        /// <summary>
        /// Sends a POST request to the server and parses the response a JSON.
        /// </summary>
        /// <param name="client"><seealso cref="HttpClient"/> to send the request using it.</param>
        /// <param name="url">URL of the API to make the request to.</param>
        /// <param name="body">Optional body request.</param>
        /// <returns>JObject containing the parsed response.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException">Thrown if a non-success result is returned from the server.</exception>

        public async static Task<JToken> RestPost(this HttpClient client, string url, object body = null)
        {
            return await client.RestCall(HttpMethod.Post, url, body);
        }

        /// <summary>
        /// Sends a DELETE request to the server and parses the response a JSON.
        /// </summary>
        /// <param name="client"><seealso cref="HttpClient"/> to send the request using it.</param>
        /// <param name="url">URL of the API to make the request to.</param>
        /// <param name="body">Optional body request.</param>
        /// <returns>JObject containing the parsed response.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException">Thrown if a non-success result is returned from the server.</exception>
        public async static Task<JToken> RestDelete(this HttpClient client, string url, object body = null)
        {
            return await client.RestCall(HttpMethod.Delete, url, body);
        }

        /// <summary>
        /// Sends a HTTP request to the server and parses the response a JSON.
        /// </summary>
        /// <param name="client"><seealso cref="HttpClient"/> to send the request using it.</param>
        /// <param name="method">HTTP method used in request.</param>
        /// <param name="url">URL of the API to make the request to.</param>
        /// <param name="body">Optional body request.</param>
        /// <returns>JObject containing the parsed response.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException">Thrown if a non-success result is returned from the server.</exception>
        private async static Task<JToken> RestCall(this HttpClient client, HttpMethod method, string url, object body = null)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = (body != null) ? new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json") : null
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (string.IsNullOrEmpty(responseStr))
                return null;
            else
                return JToken.Parse(responseStr);
        }
    }
}



