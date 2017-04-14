namespace DataLayer.CubeMonitoring
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADUsers
    {
        public int id { get; set; }

        [StringLength(150)]
        public string CN { get; set; }

        [StringLength(200)]
        public string Sid { get; set; }

        [StringLength(200)]
        public string GroupName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Added { get; set; }
    }
}
