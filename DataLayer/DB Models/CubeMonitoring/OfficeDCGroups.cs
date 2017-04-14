namespace DataLayer.CubeMonitoring
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OfficeDCGroups
    {
        public int id { get; set; }

        [StringLength(150)]
        public string GroupName { get; set; }

        public DateTime? GroupDateModified { get; set; }

        [Column(TypeName = "text")]
        public string GroupDescription { get; set; }

        [StringLength(300)]
        public string GroupPath { get; set; }
    }
}
