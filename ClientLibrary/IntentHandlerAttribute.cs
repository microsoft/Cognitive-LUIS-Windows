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

namespace Microsoft.Cognitive.LUIS
{
    /// <summary>
    /// Attribute to mark methods that can be used by an IntentRouter to handle intents.
    /// Method signature must match the <see cref="Microsoft.LUIS.Client.IntentHandlerFunc"/> delegate.
    /// </summary>
    /// <remarks>
    /// Currently, only static functions are supported.
    /// </remarks>
    /// <seealso cref="Microsoft.LUIS.Client.IntentRouter"/>
    [AttributeUsage(AttributeTargets.Method)]
    public class IntentHandlerAttribute : Attribute
    {
        /// <summary>
        /// Confidence threshold that must be reached for the handler to be activated.
        /// </summary>
        public readonly double ConfidenceThreshold;

        /// <summary>
        /// Name of the Intent defined in the LUIS server.
        /// Will use the method name if not set.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Attribute to mark methods that can be used by an IntentRouter to handle intents.
        /// </summary>
        /// <param name="confidenceThreshold">Confidence score needed to activate handler.</param>
        public IntentHandlerAttribute(double confidenceThreshold)
        {
            ConfidenceThreshold = confidenceThreshold;
        }
    }
}
