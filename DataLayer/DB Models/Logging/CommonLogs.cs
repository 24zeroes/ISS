namespace DataLayer.DB_Models.Logging
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CommonLogs
    {
        public int id { get; set; }

        public DateTime? date { get; set; }

        public string message { get; set; }

        public string application { get; set; }

        public string context { get; set; }
    }
}
