using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sodium;

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
