namespace DataLayer.RTS_CA_Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ips
    {
        public int id { get; set; }

        public int cert_center { get; set; }

        [StringLength(250)]
        public string addres { get; set; }

        [StringLength(100)]
        public string phone { get; set; }

        public virtual cert_centers cert_centers { get; set; }
    }
}
