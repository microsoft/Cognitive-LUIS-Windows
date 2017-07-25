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
    public class ApplicationHandler : LuisHandler
    {
        /// <summary>
        /// Creates a application LUIS service handler.
        /// </summary>
        /// <param name="subscriptionKey">The subscription key of the LUIS account.</param>
        /// <param name="baseApiUrl">Root URI for the service endpoint.</param>
        public ApplicationHandler(string subscriptionKey, string baseApiUrl) : base(subscriptionKey, baseApiUrl) { }


        /// <summary>
        /// Creates a new application in a LUIS subscription.
        /// </summary>
        /// <param name="name">Name of the application.</param>
        /// <param name="description">Description of the application.</param>
        /// <param name="culture">Culture language used in LUIS model.</param>
        /// <returns>The created Luis application.</returns>
        public async Task<LuisApplication> CreateApplicationAsync(string name, string description = "", string culture = Culture.English)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var path = "apps";
            var uri = CreateHandlerUri(path);

            var app = new Application()
            {
                Name = name,
                Description = description,
                Culture = culture.ToString()
            };
            var response = await _httpClient.RestPost(uri, app);
            var id = new Guid(response.ToString());
            return new LuisApplication(id, name, _subscriptionKey);
        }

        /// <summary>
        /// Searches an existing LUIS application by its name.
        /// </summary>
        /// <param name="name">Name of LUIS application to be searched.</param>
        /// <returns>The related Luis application.</returns>
        public async Task<LuisApplication> GetApplicationAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var path = "apps";
            var uri = CreateHandlerUri(path);

            var response = await _httpClient.RestGet(uri);
            var applications = response.ToObject<IEnumerable<Application>>();

            var app = (from a in applications
                       where a.Name.ToLower() == name.ToLower()
                       select new LuisApplication(a.Id, name, _subscriptionKey)).First();

            return app;
        }

        /// <summary>
        /// Gets an existing LUIS application by its Id.
        /// </summary>
        /// <param name="id">Id of LUIS application.</param>
        /// <returns>The related Luis application.</returns>
        public async Task<LuisApplication> GetApplicationAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            var path = $"apps/{id.ToString()}";
            var uri = CreateHandlerUri(path);

            var response = await _httpClient.RestGet(uri);
            var app = response.ToObject<Application>();

            return new LuisApplication(app.Id, app.Name, _subscriptionKey);
        }

        /// <summary>
        /// Deletes an existing LUIS application.
        /// </summary>
        /// <param name="id">The application Id to be deleted.</param>
        /// <returns>True if the operation successes.</returns>
        public async Task<bool> DeleteApplicationAsync(string id)
        {
            return await DeleteApplicationAsync(new Guid(id));
        }

        /// <summary>
        /// Deletes an existing LUIS application.
        /// </summary>
        /// <param name="id">The application Id to be deleted.</param>
        /// <returns>True if the operation successes.</returns>
        public async Task<bool> DeleteApplicationAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            var path = $"apps/{id}";
            var uri = CreateHandlerUri(path);

            var responseMessage = await _httpClient.RestDelete(uri);

            return true;
        }
    }
}
