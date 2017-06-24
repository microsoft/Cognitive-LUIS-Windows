using System;

namespace Microsoft.Cognitive.LUIS.Manager
{
    public class Intent
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public int typeId { get; set; }
        public string ReadableType { get; set; }

    }
}
