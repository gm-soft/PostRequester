using System.Collections.Generic;

namespace PostProcessor.Entitites
{
    public class Request
    {
        public string Url { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }
}