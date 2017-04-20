namespace DataLayer.DB_Models.CubeMonitoring
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OfficeDCUsers
    {
        public int id { get; set; }

        [StringLength(150)]
        public string UserFIO { get; set; }

        [StringLength(150)]
        public string UserSID { get; set; }

        public DateTime? UserDateModified { get; set; }

        [StringLength(300)]
        public string UserPath { get; set; }

        public int? TelephoneBookId { get; set; }
        public int DomainId { get; set; }
    }
}
