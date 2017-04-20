using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Application_Models.DC_Parser;
using DataLayer.DB_Models.CubeMonitoring;
using Newtonsoft.Json.Linq;

namespace Production
{
    public partial class DCParser
    {
        private void GetDomainGroups(JToken Domain)
        {
            DirectoryEntry entry;


            entry = new DirectoryEntry("LDAP://" + Domain.First["DC"].Value<string>(), Domain.First["Username"].Value<string>(), Domain.First["Password"].Value<string>());


            DirectorySearcher ds = new DirectorySearcher(entry);

            ds.Filter = "(&(objectClass=group))";
            try
            {
                Groups.Add(new FullDomain
                {
                    DomainName = Domain.Path.Replace("DCParser.", ""),
                    DirectorySearcher = ds,
                    SearchResult = ds.FindAll(),
                    GroupList = new List<Group>()
                });
                log.Info("Connected to domain sucessfull", Domain);
            }
            catch (Exception ex)
            {
                log.Exception("Connection to domain failed. Exception message:" + ex.Message, this.ToString(), Domain);
            }
        }

        private void UpdateDomainGroups(FullDomain Domain)
        {
            log.Info($"Domain {Domain.DomainName} group update started", ToString());

            foreach (SearchResult group in Domain.SearchResult)
            {
                using (var db = new CubeMonitoring(cubeConnectionString))
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
                            DomainId = Domain.DomainId,
                            GroupDateModified = DateTime.Now,
                            GroupPath = Path,

                        };
                        db.Entry(CurrentGroup).State = EntityState.Added;
                        db.SaveChanges();

                    }

                    Domain.GroupList.Add(new Group
                    {
                        Id = CurrentGroup.id,
                        Name = CurrentGroup.GroupName
                    });
                }



            }

            log.Info($"Domain {Domain.DomainName} group update complited", ToString());
        }
    }
}
