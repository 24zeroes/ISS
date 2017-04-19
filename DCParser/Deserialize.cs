using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production
{
    class Deserialize
    {
    }

    public class RTS
    {
        public string DC { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class GZDOM
    {
        public string DC { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class VE
    {
        public string DC { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class DCConfig
    {
        public RTS RTS { get; set; }
        public GZDOM GZDOM { get; set; }
        public VE VE { get; set; }
    }
}
