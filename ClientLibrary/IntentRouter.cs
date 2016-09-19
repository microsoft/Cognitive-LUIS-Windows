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
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    /// <summary>
    /// Delegate for intent handler functions to be routed to by an <see cref="IntentRouter"/>
    /// </summary>
    /// <param name="originalQuery">The text provided for the original query to the LUIS service.</param>
    /// <param name="entities">Dictionary containing <see cref="Entity"/> objects identified by the LUIS service.</param>
    /// <param name="context">Opaque content provided by the application to its intent handlers.</param>
    /// <returns>True if the intent was handled.</returns>
    public delegate void IntentHandlerFunc(LuisResult result, object context);

    /// <summary>
    /// Routes results returns from the LUIS service to registed intent handlers.
    /// </summary>
    public class IntentRouter : IDisposable
    {
        private class HandlerDetails
        {
            public double Threshold { get; set; }
            public IntentHandlerFunc Exec { get; set; }
        };

        private readonly Dictionary<string, HandlerDetails> _handlers = new Dictionary<string, HandlerDetails>();
        private readonly LuisClient _client;

        /// <summary>
        ///  Constructs an <see cref="IntentRouter"/> using an appID and appKey
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application</param>
        /// <param name="appKey">The application subscription key of the LUIS application</param>
        /// <param name="preview">A flag indicating whether to use preview features or not (Dialogue)</param>
        public IntentRouter(string appId, string appKey, bool preview = true)
        {
            _client = new LuisClient(appId, appKey, preview);
        }

        /// <summary>
        ///  Constructs an <see cref="IntentRouter"/> using an existing LUIS client
        /// </summary>
        /// <param name="client">The instance of the LuisClient to use</param>
        public IntentRouter(LuisClient client)
        {
            _client = client;
        }


        /// <summary>
        /// Set up a new instance of an <see cref="IntentRouter"/> using <see cref="IntentHandlerAttribute"/> flagged methods in <typeparamref name="T"/> to handle the intents.
        /// </summary>
        /// <typeparam name="T">Type of handler</typeparam>
        /// <param name="appId">The application ID of the LUIS application</param>
        /// <param name="appKey">The application subscription key of the LUIS application</param>
        /// <param name="preview">A flag indicating whether to use preview features or not (Dialogue)</param>
        /// <returns>A new instance of an <see cref="IntentRouter"/> that has registered intent handlers.</returns>
        public static IntentRouter Setup<T>(string appId, string appKey, bool preview = true)
        {
            var router = new IntentRouter(appId, appKey, preview);
            try
            {
                router.RegisterHandler<T>();
            }
            catch
            {
                router.Dispose();
                throw;
            }
            return router;
        }

        /// <summary>
        /// Set up a new instance of an <see cref="IntentRouter"/> using <see cref="IntentHandlerAttribute"/> flagged methods in <typeparamref name="T"/> to handle the intents.
        /// </summary>
        /// <typeparam name="T">Type of handler</typeparam>
        /// <param name="client">The instance of the LuisClient to use</param>
        /// <returns>A new instance of an <see cref="IntentRouter"/> that has registered intent handlers.</returns>
        public static IntentRouter Setup<T>(LuisClient client)
        {
            var router = new IntentRouter(client);
            try
            {
                router.RegisterHandler<T>();
            }
            catch
            {
                router.Dispose();
                throw;
            }
            return router;
        }

        /// <summary>
        /// Set up a new instance of an <see cref="IntentRouter"/> using <see cref="IntentHandlerAttribute"/> flagged methods to handle the intents.
        /// </summary>
        /// <param name="appId">The application ID of the LUIS application</param>
        /// <param name="appKey">The application subscription key of the LUIS application</param>
        /// <param name="contextObject">The instance of the class that contains the flagged member functions to execute for each intent</param>
        /// <param name="preview">A flag indicating whether to use preview features or not (Dialogue)</param>
        /// <returns>A new instance of an <see cref="IntentRouter"/> that has registered intent handlers.</returns>
        public static IntentRouter Setup(string appId, string appKey, object contextObject, bool preview = true)
        {
            var router = new IntentRouter(appId, appKey, preview);
            try
            {
                router.RegisterHandler(contextObject);
            }
            catch
            {
                router.Dispose();
                throw;
            }
            return router;
        }

        /// <summary>
        /// Set up a new instance of an <see cref="IntentRouter"/> using <see cref="IntentHandlerAttribute"/> flagged methods to handle the intents.
        /// </summary>
        /// <typeparam name="T">Type of handler</typeparam>
        /// <param name="client">The instance of the LuisClient to use</param>
        /// <param name="contextObject">The instance of the class that contains the flagged member functions to execute for each intent</param>
        /// <returns>A new instance of an <see cref="IntentRouter"/> that has registered intent handlers.</returns>
        public static IntentRouter Setup(LuisClient client, object contextObject)
        {
            var router = new IntentRouter(client);
            try
            {
                router.RegisterHandler(contextObject);
            }
            catch
            {
                router.Dispose();
                throw;
            }
            return router;
        }

        /// <summary>
        /// Register an intent handler with this <see cref="IntentRouter"/>.
        /// </summary>
        /// <param name="intentName">Name of the intent as defined in the LUIS application.</param>
        /// <param name="confidenceThreshold">Confidence score required to accitvate the handler.</param>
        /// <param name="handler">Function to call when routing the intent to this handler.</param>
        public void RegisterHandler(string intentName, double confidenceThreshold, IntentHandlerFunc handler)
        {
            if (string.IsNullOrWhiteSpace(intentName)) throw new ArgumentException(nameof(intentName));
            if ((confidenceThreshold < 0) || (confidenceThreshold > 1)) throw new ArgumentOutOfRangeException(nameof(confidenceThreshold));
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (_handlers.ContainsKey(intentName)) throw new InvalidOperationException($"Router already has a handler for intent '{intentName}'");

            _handlers[intentName] = new HandlerDetails()
            {
                Threshold = confidenceThreshold,
                Exec = handler
            };
        }

        /// <summary>
        /// Scans <typeparamref name="T"/> for all method flagged with <see cref="IntentHandlerAttribute"/> and registers them with this <see cref="IntentRouter"/>
        /// </summary>
        /// <typeparam name="T">Class to scan for intent handlers.</typeparam>
        public void RegisterHandler(object contextObject)
        {

            foreach (var method in contextObject.GetType().GetRuntimeMethods())
            {
                var intenthandler = method.GetCustomAttribute<IntentHandlerAttribute>();
                if (intenthandler != null)
                {
                    if (method.IsStatic)
                    {
                        var handlerFunc = (IntentHandlerFunc)method.CreateDelegate(typeof(IntentHandlerFunc));
                        RegisterHandler(intenthandler.Name ?? method.Name, intenthandler.ConfidenceThreshold, handlerFunc);
                    }
                    else
                    {
                        var handlerFunc = (IntentHandlerFunc)method.CreateDelegate(typeof(IntentHandlerFunc), contextObject);
                        RegisterHandler(intenthandler.Name ?? method.Name, intenthandler.ConfidenceThreshold, handlerFunc);
                    }
                }

            }
        }

        /// <summary>
        /// Scans <paramref name="handlerClass"/> for all method flagged with <see cref="IntentHandlerAttribute"/> and registers them with this <see cref="IntentRouter"/>
        /// </summary>
        /// <param name="handlerClass">Class to scan for intent handlers.</param>
        public void RegisterHandler<T>()
        {
            Type handlerClass = typeof(T);
            foreach (var method in handlerClass.GetRuntimeMethods())
            {
                if (method.IsStatic)
                {
                    var intenthandler = method.GetCustomAttribute<IntentHandlerAttribute>();
                    if (intenthandler != null)
                    {
                        var handlerFunc = (IntentHandlerFunc)method.CreateDelegate(typeof(IntentHandlerFunc));
                        RegisterHandler(intenthandler.Name ?? method.Name, intenthandler.ConfidenceThreshold, handlerFunc);
                    }
                }
            }
        }

        /// <summary>
        /// Makes a request to the LUIS service to parse <paramref name="text"/> for intents and entities.
        /// </summary>
        /// <param name="text">Natural language text string to parse.</param>
        /// <param name="context">Opaque context object to pass through to activated intent handler</param>
        /// <returns>True if an intent was routed and handled.</returns>
        public async Task<bool> Route(string text, object context)
        {
            if (_client == null) throw new InvalidOperationException("No API endpoint has been specified for this IntentRouter");

            LuisResult result = result = await _client.Predict(text);
            return Route(result, context);
        }

        /// <summary>
        /// Route an existing LUIS result through to a matching intent handler.
        /// </summary>
        /// <param name="result">Result from LUIS service to route.</param>
        /// <param name="context">Opaque context object to pass through to activated intent handler</param>
        /// <returns>True if an intent was routed and handled.</returns>
        public bool Route(LuisResult result, object context)
        {
            if (result.Intents == null)
                return false;

            foreach (var intent in result.Intents)
            {
                HandlerDetails handler;
                if (_handlers.TryGetValue(intent.Name, out handler) && (handler.Threshold < intent.Score))
                {
                    handler.Exec(result, context);
                    return true;
                }
            }

            return false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_client != null)
                    {
                        _client.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Disposes this <see cref="IntentRouter"/> and associated managed objects.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
