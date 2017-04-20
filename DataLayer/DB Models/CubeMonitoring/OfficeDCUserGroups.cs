namespace DataLayer.DB_Models.CubeMonitoring
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OfficeDCUserGroups
    {
        public int id { get; set; }

        public int? UserId { get; set; }

        public int? GroupId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int DomainId { get; set; }
    }
}
