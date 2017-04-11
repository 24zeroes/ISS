namespace ISS.DB_Models.RTS_CA_Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user_certs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user_certs()
        {
            users = new HashSet<users>();
        }

        public int id { get; set; }

        [StringLength(100)]
        public string user_serial { get; set; }

        [StringLength(100)]
        public string user_hash { get; set; }

        [StringLength(100)]
        public string root_serial { get; set; }

        [StringLength(100)]
        public string root_hash { get; set; }

        [StringLength(100)]
        public string OID { get; set; }

        public int? hex_id { get; set; }

        public int? root { get; set; }

        public virtual root_cert root_cert { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<users> users { get; set; }
    }
}
