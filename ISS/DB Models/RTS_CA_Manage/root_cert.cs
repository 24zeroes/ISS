namespace ISS.DB_Models.RTS_CA_Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class root_cert
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public root_cert()
        {
            user_certs = new HashSet<user_certs>();
        }

        public int id { get; set; }

        public int? cert_center { get; set; }

        [StringLength(100)]
        public string serial { get; set; }

        [StringLength(100)]
        public string hash { get; set; }

        [StringLength(100)]
        public string url { get; set; }

        [StringLength(150)]
        public string url_crl { get; set; }

        public string hex { get; set; }

        [StringLength(100)]
        public string ident { get; set; }

        public bool? self_signed { get; set; }

        [Column(TypeName = "date")]
        public DateTime? added { get; set; }

        public virtual cert_centers cert_centers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_certs> user_certs { get; set; }
    }
}
