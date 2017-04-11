namespace ISS.DB_Models.RTS_CA_Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class mails
    {
        public int id { get; set; }

        public string text { get; set; }

        public int? alert_id { get; set; }

        public virtual alerts alerts { get; set; }
    }
}
