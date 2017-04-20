using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Application_Models.DC_Parser;
using DataLayer.DB_Models.CubeMonitoring;

namespace Production
{
    public partial class DCParser
    {
        private void UpdateUserGroups(FullDomain Domain)
        {
            log.Info($"Domain {Domain.DomainName} UserGroups update started", ToString());
            using (var db = new CubeMonitoring(cubeConnectionString))
            {
                foreach (var group in Domain.GroupList)
                {
                    if (group.Id != null)
                    {
                        Domain.DirectorySearcher.Filter = $"(&(cn={group.Name})(objectClass=group))";
                        Domain.DirectorySearcher.PropertiesToLoad.Add("member");
                        Domain.SearchResult = Domain.DirectorySearcher.FindAll();
                        if (Domain.SearchResult.Count != 0)
                        {
                            foreach (var UserCn in Domain.SearchResult[0].Properties["member"])
                            {
                                string cn = UserCn.ToString();
                                cn = cn.Substring(3, cn.IndexOf(",") - 3);
                                var CurrentUser = db.OfficeDCUsers.FirstOrDefault(g => g.UserFIO == cn);
                                if (CurrentUser != null)
                                {
                                    var UserGroup =
                                        db.OfficeDCUserGroups.FirstOrDefault(
                                            (g => (g.GroupId == group.Id) && (g.UserId == CurrentUser.id)));
                                    if (UserGroup == null)
                                    {
                                        var NewUserGroup = new OfficeDCUserGroups
                                        {
                                            UserId = CurrentUser.id,
                                            GroupId = group.Id,
                                            DomainId = Domain.DomainId,
                                            ModifiedDate = DateTime.Now
                                        };
                                        db.Entry(NewUserGroup).State = EntityState.Added;

                                    }
                                }
                            }
                        }
                    }

                }
                db.SaveChanges();
            }
            log.Info($"Domain {Domain.DomainName} UserGroups update complited", ToString());
        }
    }
}
