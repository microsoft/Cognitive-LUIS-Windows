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
    /// Represents a composite entity recognised by LUIS
    /// </summary>
    public class CompositeEntity
    {
        /// <summary>
        /// The name of the type of parent entity.
        /// </summary>
        public string ParentType { get; set; }
        /// <summary>
        /// The composite entity value.
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// A list of child entities of the composite entity.
        /// </summary>
        public CompositeEntityChild[] CompositeEntityChildren { get; set; }

        /// <summary>
        /// Loads the json object into the properties of the object.
        /// </summary>
        /// <param name="compositeEntity">Json object containing the composite entity</param>
        public void Load(JObject compositeEntity)
        {
            ParentType = (string)compositeEntity["parentType"];
            Value = (string)compositeEntity["value"];
            try
            {
                var values = (JArray)compositeEntity["children"] ?? new JArray();
                CompositeEntityChildren = ParseValuesArray(values);
            }
            catch (Exception)
            {
                CompositeEntityChildren = null;
            }

        }

        /// <summary>
        /// Parses Json array of composite entity children into composite entity child array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns>entities array</returns>
        private CompositeEntityChild[] ParseValuesArray(JArray array)
        {
            var count = array.Count;
            var a = new CompositeEntityChild[count];
            for (var i = 0; i < count; i++)
            {
                var t = new CompositeEntityChild();
                t.Load((JObject)array[i]);
                a[i] = t;
            }
            return a;
        }
    }
}
