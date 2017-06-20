using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.LUIS
{
    public class Examples
    {
        private readonly HttpClient _http;

        public Examples(HttpClient http)
        {
            _http = http;
        }

        public async Task<Label> AddLabelAsync(LabelRequest label)
        {
            throw new NotImplementedException();
        }

        public async Task<Label> AddLabelsAsync(IEnumerable<LabelRequest> labels)
        {
            throw new NotImplementedException();
        }
    }
}
