using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DB_Models.ISS
{
    public partial class Config
    {
        public int id { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
    }
}
