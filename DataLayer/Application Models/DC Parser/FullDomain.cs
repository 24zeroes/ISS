using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Application_Models.DC_Parser
{
    public class FullDomain
    {
        public string DomainName { get; set; }
        public int DomainId { get; set; }
        public DirectorySearcher DirectorySearcher { get; set; }
        public SearchResultCollection SearchResult { get; set; }
        public List<Group> GroupList { get; set; }
    }
}
