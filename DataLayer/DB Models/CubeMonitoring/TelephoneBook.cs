namespace DataLayer.CubeMonitoring
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TelephoneBook")]
    public partial class TelephoneBook
    {
        public int id { get; set; }

        [StringLength(300)]
        public string FIO { get; set; }

        public int? Telephone { get; set; }

        [StringLength(300)]
        public string OrgUnit { get; set; }

        [StringLength(300)]
        public string Position { get; set; }

        [StringLength(300)]
        public string Organisation { get; set; }

        [StringLength(300)]
        public string Email { get; set; }

        [StringLength(300)]
        public string MobileTelephone { get; set; }
    }
}
