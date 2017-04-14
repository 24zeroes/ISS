namespace DataLayer.RTS_CA_Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class alerts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public alerts()
        {
            mails = new HashSet<mails>();
        }

        public int id { get; set; }

        public int subject { get; set; }

        [StringLength(100)]
        public string server { get; set; }

        [StringLength(100)]
        public string error_1 { get; set; }

        [StringLength(100)]
        public string error_2 { get; set; }

        public DateTime? date { get; set; }

        [StringLength(200)]
        public string cert_center { get; set; }

        [StringLength(100)]
        public string root_serial { get; set; }

        [StringLength(100)]
        public string ident { get; set; }

        public string reason { get; set; }

        public virtual users users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mails> mails { get; set; }
    }
}
