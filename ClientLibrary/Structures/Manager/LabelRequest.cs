using System.Collections.Generic;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class LabelRequest
    {
        public string Text { get; set; }
        public string IntentName { get; set; }
        public IEnumerable<EntityLabel> EntityLabels { get; set; }
    }
}
