namespace ISS.DB_Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OfficeDCEvents
    {
        public int id { get; set; }

        [StringLength(500)]
        public string EventName { get; set; }

        [StringLength(150)]
        public string LogName { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(150)]
        public string Task { get; set; }

        [StringLength(150)]
        public string IpAddress { get; set; }

        [StringLength(150)]
        public string IpPort { get; set; }

        [StringLength(150)]
        public string SubjectUserSid { get; set; }

        [StringLength(150)]
        public string SubjectUserName { get; set; }

        [StringLength(150)]
        public string SubjectDomainName { get; set; }

        [StringLength(150)]
        public string TargetUserSid { get; set; }

        [StringLength(150)]
        public string TargetUserName { get; set; }

        [StringLength(150)]
        public string TargetDomainName { get; set; }

        [StringLength(150)]
        public string PackageName { get; set; }

        [StringLength(150)]
        public string Workstation { get; set; }

        [StringLength(150)]
        public string Computer { get; set; }

        public int? EventId { get; set; }

        [StringLength(150)]
        public string DnsHostName { get; set; }

        [StringLength(150)]
        public string EventThumb { get; set; }

        public int? GroupMemberId { get; set; }
    }
}
