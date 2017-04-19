using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Application_Models.DC_Parser
{
    public class FullGroups
    {
        public string DomainName { get; set; }
        public SearchResultCollection SearchResult { get; set; }
        public List<Group> GroupList { get; set; }
    }
}
