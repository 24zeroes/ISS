namespace DataLayer.RTS_CA_Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class mails_backup
    {
        public int id { get; set; }

        public string text { get; set; }

        public int? alert_id { get; set; }
    }
}
