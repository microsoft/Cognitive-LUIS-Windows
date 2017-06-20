using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    public class LabelRequest
    {
        public string Text { get; set; }
        public string IntentName { get; set; }
        public IEnumerable<EntityLabel> EntityLabels { get; set; }
    }
}
