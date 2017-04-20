using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Application_Models.DC_Parser;
using DataLayer.DB_Models.CubeMonitoring;

namespace Production
{
    public partial class DCParser
    {
        private void UpdateUsers(FullDomain Domain)
        {
            log.Info($"Domain {Domain.DomainName} user update started", ToString());
            Domain.DirectorySearcher.Filter = "(&(objectClass=user))";
            Domain.SearchResult = Domain.DirectorySearcher.FindAll();
            foreach (SearchResult user in Domain.SearchResult)
            {
                using (var db = new CubeMonitoring(cubeConnectionString))
                {
                    string sid;
                    try
                    {
                        byte[] temp1 = (byte[]) user.Properties["objectSid"][0];
                        SecurityIdentifier temp = new SecurityIdentifier(temp1, 0);
                        sid = temp.ToString();
                    }
                    catch (Exception)
                    {
                        sid = null;

                    }
                    string UserSID = sid;
                    string UserFIO = user.Properties["cn"][0].ToString();
                    string UserPath = user.Path;

                    var CurrentUser = db.OfficeDCUsers.FirstOrDefault(g => g.UserSID == UserSID);

                    if (CurrentUser != null)
                    {
                        if (CurrentUser.UserFIO != UserFIO)
                        {
                            CurrentUser.UserFIO = UserFIO;
                            CurrentUser.UserDateModified = DateTime.Now;
                            db.Entry(CurrentUser).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        if (CurrentUser.TelephoneBookId == null)
                        {
                            int surnameI = CurrentUser.UserFIO.IndexOf(" ");
                            int nameI = (surnameI == -1) ? -1 : CurrentUser.UserFIO.LastIndexOf(" ");

                            string surname = (surnameI == -1) ? "" : CurrentUser.UserFIO.Substring(0, surnameI);
                            string name = (nameI == -1)
                                ? ""
                                : CurrentUser.UserFIO.Substring(surnameI + 1,
                                    CurrentUser.UserFIO.Length - (surnameI + nameI));
                            string patronymic = (nameI == -1)
                                ? ""
                                : CurrentUser.UserFIO.Substring(nameI + 1, CurrentUser.UserFIO.Length - (nameI + 1));


                            if (((surnameI != -1) && (nameI == -1)) || ((surnameI != -1) && (nameI != -1)))
                            {
                                var TelUser =
                                    db.TelephoneBook.FirstOrDefault(
                                        u =>
                                            (u.FIO.Contains(surname) &&
                                             u.FIO.Contains(name)));

                            }


                        }
                    }
                    else
                    {

                        var User = new OfficeDCUsers
                        {
                            UserFIO = UserFIO,
                            UserSID = UserSID,
                            UserPath = UserPath,
                            DomainId = Domain.DomainId,
                            UserDateModified = DateTime.Now
                        };
                        db.Entry(User).State = EntityState.Added;
                        db.SaveChanges();

                    }
                }
            }
            log.Info($"Domain {Domain.DomainName} user update complited", ToString());
        }
    }
}
