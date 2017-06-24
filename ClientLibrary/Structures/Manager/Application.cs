using System;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class Application
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Culture { get; set; }
        public string UsageScenario { get; set; }
        public string Domain { get; set; }
        public string InitialVersionId { get; set; }
    }
}
