using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Application_Models.ISS
{
    public class Task
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public long ExecCount { get; set; }
        public string Rule {get;set;}
        
    }
}
