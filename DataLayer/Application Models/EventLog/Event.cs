using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Application_Models.EventLog
{
    public class Event
    {
        public int Id { get; set; }
        public string LogName { get; set; }
        public DateTime Date { get; set; }
        public string Task { get; set; }
        public string IpAddress { get; set; }
        public string IpPort { get; set; }
        public string SubjectUserSid { get; set; }
        public string SubjectUserName { get; set; }
        public string SubjectDomainName { get; set; }
        public string TargetUserSid { get; set; }
        public string TargetUserName { get; set; }
        public string TargetDomainName { get; set; }
        public string PackageName { get; set; }
        public string Workstation { get; set; }
    }
}
