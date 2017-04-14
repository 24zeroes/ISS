namespace DataLayer.CubeMonitoring
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OfficeDCComputers
    {
        public int id { get; set; }

        [StringLength(150)]
        public string ComputerName { get; set; }

        [StringLength(150)]
        public string ComputerIP { get; set; }

        public DateTime? ComputerDateModified { get; set; }
    }
}
