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

    /// <summary>
    /// Reresents the results of a natural langage string parse by the LUIS service.
    /// </summary>
    public class LuisResult
    {
        public Func<string, LuisResult> Reply { get; }

        /// <summary>
        /// Original text sent to the LUIS service for parsing.
        /// </summary>
        public string OriginalQuery { get; set; }        

        /// <summary>
        /// Original text that has been spell-corrected with Luis-Bing integration.
        /// </summary>
        /// <remarks>Only has a value if the original query was changed.</remarks>
        public string AlteredQuery { get; set; }

        /// <summary>
        /// The best matching intent in this result set.
        /// </summary>
        public Intent TopScoringIntent { get; set; }

        /// <summary>
        /// contains the dialog prompt and context (if exists)
        /// </summary>
        public Dialog DialogResponse { get; set; }

        /// <summary>
        /// List of <see cref="Intent"/> objects parsed.
        /// </summary>
        public Intent[] Intents { get; set; }

        /// <summary>
        /// Checks whether the result is awaiting more dialog or not
        /// </summary>
        /// <returns>boolean indicating whether the LuisResult awaits a dialog or not</returns>
        public bool isAwaitingDialogResponse()
        {
            return (DialogResponse != null && 
                string.Compare(DialogResponse.Status, DialogStatus.Finished, StringComparison.OrdinalIgnoreCase) != 0);
        }

        /// <summary>
        /// Collection of <see cref="Entity"/> objects parsed accessed though a dictionary for easy access.
        /// </summary>
        public IDictionary<string, IList<Entity>> Entities { get; set; }

        /// <summary>
        /// Collection of <see cref="CompositeEntities"/> objects parsed accessed though a dictionary for easy access.
        /// </summary>
        public IDictionary<string, IList<CompositeEntity>> CompositeEntities { get; set; }

        /// <summary>
        /// Construct an empty result set.
        /// </summary>
        public LuisResult() { }

        /// <summary>
        /// Contruce a result set based on the JSON response from the LUIS service.
        /// </summary>
        /// <param name="client">The client of which we can use to Reply</param>
        /// <param name="result">The parsed JSON from the LUIS service.</param>
        public LuisResult(LuisClient client, JToken result)
        {
            var luisResult = this;
            this.Reply = new Func<string, LuisResult>((text) => client.Reply(luisResult, text).GetAwaiter().GetResult());
            Load(result);
        }

        /// <summary>
        /// Loads parsing results into this result set from the <see cref="JObject"/>.
        /// </summary>
        /// <param name="result"></param>
        public void Load(JToken result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            OriginalQuery = (string)result["query"] ?? string.Empty;
            AlteredQuery = (string)result["alteredQuery"] ?? string.Empty;
            var intents = (JArray)result["intents"] ?? new JArray();
            TopScoringIntent = new Intent();
            TopScoringIntent.Load((JObject)result["topScoringIntent"]);
            Intents = ParseIntentArray(intents);
            if (Intents.Length == 0)
            {
                Intents = new Intent[1];
                Intents[0] = TopScoringIntent;
            }
            if (result["dialog"] != null)
            {
                var t = new Dialog();
                t.Load((JObject)result["dialog"]);
                DialogResponse = t;
            }
            var entities = (JArray)result["entities"] ?? new JArray();
            Entities = ParseEntityArrayToDictionary(entities);
            var compositeEntities = (JArray)result["compositeEntities"] ?? new JArray();
            CompositeEntities = ParseCompositeEntityArrayToDictionary(compositeEntities);
        }

        /// <summary>
        /// gets all entities returned by the LUIS service
        /// </summary>
        /// <returns>a list of all entities</returns>
        public List<Entity> GetAllEntities()
        {
            List<Entity> entities = new List<Entity>();
            foreach (var entityList in Entities)
            {
                foreach (Entity entity in entityList.Value)
                {
                    entities.Add(entity);
                }
            }
            return entities;
        }

        /// <summary>
        /// gets all composite entities returned by the LUIS service
        /// </summary>
        /// <returns>a list of all composite entities</returns>
        public List<CompositeEntity> GetAllCompositeEntities()
        {
            List<CompositeEntity> compositeEntities = new List<CompositeEntity>();
            foreach (var compositeEntityList in CompositeEntities)
            {
                foreach (CompositeEntity compositeEntity in compositeEntityList.Value)
                {
                    compositeEntities.Add(compositeEntity);
                }
            }
            return compositeEntities;
        }

        /// <summary>
        /// Parses a json array of intents into an intent array
        /// </summary>
        /// <param name="array">Json array containing intents</param>
        /// <returns>Intent array</returns>
        private Intent[] ParseIntentArray(JArray array)
        {
            var count = array.Count;
            var a = new Intent[count];
            for (var i = 0; i < count; i++)
            {
                var t = new Intent();
                t.Load((JObject)array[i]);
                a[i] = t;
            }

            return a;
        }

        private IDictionary<string, IList<Entity>> ParseEntityArrayToDictionary (JArray array)
        {
            var dict = new Dictionary<string, IList<Entity>>();

            foreach (var item in array)
            {
                var e = new Entity();
                e.Load((JObject) item);

                if (ShouldIgnoreEntity(e))
                {
                    continue;
                }

                IList<Entity> entityList;
                if (!dict.TryGetValue(e.Name, out entityList))
                {
                    dict[e.Name] = new List<Entity>() { e };
                }
                else
                {
                    entityList.Add(e);
                }
            }
            return dict;
        }

        private bool ShouldIgnoreEntity (Entity e)
        {
            //Luis sometimes returns entities with "type" equal to null (mapped to "Name").
            //  Ignore these kinds of entities.
            //  For example (note how the first two entities 'add up' to the last one):
            //    "entities": [
            //    {
            //        "entity": "opportunity",
            //        "type": null,
            //        "startIndex": 18,
            //        "endIndex": 28,
            //        "score": 0.179574952
            //    },
            //    {
            //        "entity": "fund",
            //        "type": null,
            //        "startIndex": 30,
            //        "endIndex": 33,
            //        "score": 0.164282173
            //    },
            //    {
            //        "entity": "opportunity fund",
            //        "type": "Fund",
            //        "startIndex": 18,
            //        "endIndex": 33,
            //        "score": 0.996879935
            //    }
            //]
            return e.Name == null;
        }

        /// <summary>
        /// Parses a json array of composite entities into a composite entity dictionary.
        /// </summary>
        /// <param name="array"></param>
        /// <returns>The object containing the dictionary of composite entities</returns>
        private IDictionary<string, IList<CompositeEntity>> ParseCompositeEntityArrayToDictionary(JArray array)
        {
            var count = array.Count;
            var dict = new Dictionary<string, IList<CompositeEntity>>();

            foreach (var item in array)
            {
                var e = new CompositeEntity();
                e.Load((JObject)item);

                IList<CompositeEntity> compositeEntityList;
                if (!dict.TryGetValue(e.ParentType, out compositeEntityList))
                {
                    dict[e.ParentType] = new List<CompositeEntity>() { e };
                }
                else
                {
                    compositeEntityList.Add(e);
                }
            }

            return dict;
        }
    }
}
