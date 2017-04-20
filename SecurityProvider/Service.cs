using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SecurityProvider
{
    class Service
    {
        public byte[] PublicKey { get; set; }
        public string Name { get; set; }
        public byte[] Message { get; set; }
        public byte[] Nonce { get; set; }
        public List<JToken> Items { get; set; }
    }
}
