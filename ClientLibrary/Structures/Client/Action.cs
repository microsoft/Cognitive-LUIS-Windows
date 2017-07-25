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
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microsoft.Cognitive.LUIS
{
    public class Action
    {
        /// <summary>
        /// Name of the action.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Where the action is triggered or not
        /// </summary>
        public bool Triggered { get; set; }

        public Parameter[] Parameters { get; set; }
        /// <summary>
        /// Load the action values from JSON returned from the LUIS service.
        /// </summary>
        /// <param name="action">JSON containing the intent values.</param>
        public void Load(JObject action)
        {
            Name = (string)action["name"];
            Triggered = (bool)action["triggered"];
            var parameterss = (JArray)action["parameters"] ?? new JArray();
            Parameters = ParseParamArray(parameterss);
        }
        /// <summary>
        /// Parses an array of paramaeters from a json object into a parameter object array
        /// </summary>
        /// <param name="array">Json array containing the parameters</param>
        /// <returns>Parameter object array</returns>
        private Parameter[] ParseParamArray(JArray array)
        {
            var count = array.Count;
            var a = new Parameter[count];
            for (var i = 0; i < count; i++)
            {
                var t = new Parameter();
                t.Load((JObject)array[i]);
                a[i] = t;
            }
            return a;
        }
    }
}
