using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sodium;

namespace EncryptionProvider
{
    class Container
    {
        public string Name { get; set; }
        public KeyPair Keys { get; set; }
        public string InputFullPath { get; set; }

        public string OutputPath { get; set; }
    }
}
