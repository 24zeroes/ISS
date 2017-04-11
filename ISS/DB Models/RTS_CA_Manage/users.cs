namespace ISS.DB_Models.RTS_CA_Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public users()
        {
            alerts = new HashSet<alerts>();
        }

        public int id { get; set; }

        public int cert { get; set; }

        [StringLength(100)]
        public string fio { get; set; }

        [StringLength(12)]
        public string inn { get; set; }

        [StringLength(200)]
        public string org { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        public int? region_code { get; set; }

        [Column(TypeName = "text")]
        public string region_name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<alerts> alerts { get; set; }

        public virtual user_certs user_certs { get; set; }
    }
}
