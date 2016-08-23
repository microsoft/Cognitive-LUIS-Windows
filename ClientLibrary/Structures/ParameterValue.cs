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
    public class ParameterValue
    {

        /// <summary>
        /// The entity detected
        /// </summary>
        public string Entity { get; set; }
        /// <summary>
        /// The type of entity
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The resolution dictionary containing specific parameters for built-in entities
        /// </summary>
        public Dictionary<string, Object> Resolution;

        /// <summary>
        /// Loads the json object into the properties of the object
        /// </summary>
        /// <param name="parameterValue">Json object containing the parameter value</param>
        public void Load(JObject parameterValue)
        {
            Entity = (string)parameterValue["entity"];
            Type = (string)parameterValue["type"];
            try
            {
                Resolution = parameterValue["resolution"].ToObject<Dictionary<string, Object>>();
            }
            catch (Exception)
            {
                Resolution = new Dictionary<string, Object>();
            }
        }
    }
}
