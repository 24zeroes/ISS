using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Application_Models;
using DataLayer.Application_Models.DC_Parser;
using DataLayer.DB_Models.CubeMonitoring;
using Newtonsoft.Json.Linq;

namespace Production
{
    public partial class DCParser
    {
        private void GetDomainGroups(Domain domain)
        {
            DirectoryEntry entry;


            entry = new DirectoryEntry("LDAP://" + domain.DC, domain.Username, domain.Password);


            DirectorySearcher ds = new DirectorySearcher(entry);

            ds.Filter = "(&(objectClass=group))";
            try
            {
                Groups.Add(new FullDomain
                {
                    DomainName = domain.Name,
                    DirectorySearcher = ds,
                    SearchResult = ds.FindAll(),
                    GroupList = new List<Group>()
                });
                log.Info("Connected to domain sucessfull", domain);
            }
            catch (Exception ex)
            {
                log.Exception("Connection to domain failed. Exception message:" + ex.Message, this.ToString(), domain);
            }
        }

        private void UpdateDomainGroups(FullDomain domain)
        {
            log.Info($"Domain {domain.DomainName} group update started", ToString());

            foreach (SearchResult group in domain.SearchResult)
            {
                using (var db = new CubeMonitoring(DCConfig.ConnectionString))
                {
                    string Description = "";
                    string Name = group.Properties["name"][0].ToString();
                    string Path = group.Path;



                    if (group.Properties["description"].Count != 0)
                    {
                        Description = group.Properties["description"][0].ToString();
                    }

                    var CurrentGroup = db.OfficeDCGroups.FirstOrDefault(g => g.GroupName == Name);

                    if (CurrentGroup != null)
                    {
                        if (CurrentGroup.GroupDescription != Description)
                        {

                            CurrentGroup.GroupDescription = Description;
                            CurrentGroup.GroupDateModified = DateTime.Now;
                            db.Entry(CurrentGroup).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        CurrentGroup = new OfficeDCGroups
                        {
                            GroupName = Name,
                            GroupDescription = Description,
                            DomainId = domain.DomainId,
                            GroupDateModified = DateTime.Now,
                            GroupPath = Path,

                        };
                        db.Entry(CurrentGroup).State = EntityState.Added;
                        db.SaveChanges();

                    }

                    domain.GroupList.Add(new Group
                    {
                        Id = CurrentGroup.id,
                        Name = CurrentGroup.GroupName
                    });
                }



            }

            log.Info($"Domain {domain.DomainName} group update complited", ToString());
        }
    }
}
