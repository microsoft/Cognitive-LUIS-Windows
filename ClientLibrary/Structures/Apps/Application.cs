using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    public class Application
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Culture { get; set; }
        public string UsageScenario { get; set; }
        public string Domain { get; set; }
        public string InitialVersionId { get; set; }
    }
}
