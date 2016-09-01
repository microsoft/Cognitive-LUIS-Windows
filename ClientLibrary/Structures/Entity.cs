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

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Microsoft.Cognitive.LUIS
{
    /// <summary
    /// Represents an entity recognised by LUIS
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// The name of the type of Entity, e.g. "Topic", "Person", "Location".
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The entity value, e.g. "Latest scores", "Alex", "Cambridge".
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Confidence score that LUIS matched the entity, the higher the better.
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// The index of the first character of the entity within the given text
        /// </summary>
        public int StartIndex { get; set; }
        /// <summary>
        /// The index of the last character of the entity within the given text
        /// </summary>
        public int EndIndex { get; set; }
        /// <summary>
        /// The resolution dictionary containing specific parameters for built-in entities
        /// </summary>
        public Dictionary<string, Object> Resolution;

        /// <summary>
        /// Loads the Entity values from a JSON object returned from the LUIS service.
        /// </summary>
        /// <param name="entity">The JObject containing the entity values</param>
        public void Load(JObject entity)
        {
            Name = (string)entity["type"];
            Value = (string)entity["entity"];
            try
            {
                Score = (double)entity["score"];
            }
            catch (Exception)
            {
                Score = -1;
            }
            try
            {
                StartIndex = (int)entity["startIndex"];
            }
            catch(Exception)
            {
                StartIndex = -1;
            }
            try
            {
                EndIndex = (int)entity["endIndex"];
            }
            catch (Exception)
            {
                EndIndex = -1;
            }
            try
            {
                Resolution = entity["resolution"].ToObject<Dictionary<string, Object>>();
            }
            catch (Exception)
            {
                Resolution = new Dictionary<string, Object>();
            }
        }
    }
}
