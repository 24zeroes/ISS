using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DB_Models.CubeMonitoring
{
    public partial class Domains
    {
        public int id { get; set; }

        [StringLength(150)]
        public string DomainName { get; set; }
    }
}
