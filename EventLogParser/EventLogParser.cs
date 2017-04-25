using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AppPattern;
using DataLayer.DB_Models.CubeMonitoring;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataLayer.Application_Models.EventLog;

namespace Production
{
    public partial class EventLogParser : Application
    {
        [JsonIgnore]
        private JToken EventLogParserConfig;
        [JsonIgnore]
        private JToken EventLogParserDbConfig;
        [JsonIgnore]
        private string cubeConnectionString;

        private JToken EventLogParserEmail;

        private JToken DomainConfig;

        private List<EventRecord> EventRecords; 

        private List<Dictionary<string, string>> dicts;
        private string DomainName;
        private int DomainId;

        public override void GetConfiguration()
        {
            SecCore.TaskStarted(id);
            cubeConnectionString = SecCore.GetProtectedInfo("DB_Cube")["ConnectionString"].Value<string>();
            EventLogParserConfig = SecCore.GetProtectedInfo("EventLogParser")["Config"];
            EventLogParserEmail = SecCore.GetProtectedInfo("EventLogParser")["Email"];
            DomainConfig = SecCore.GetProtectedInfo("DCParser");
            log.Info("Initialisation sucessfull", this.ToString());
        }

        public override void InitialiseInputData()
        {

            GetEventRecords();
            log.Info("Forwarded events added to dict", this.ToString(), dicts.Count);

        }

        public override void ProcessData()
        {
            log.Info("Dicts parsing started", this.ToString());
            foreach (var task in dicts)
            {
                var Event = GetEventFromDict(task);

                using (var db = new CubeMonitoring(cubeConnectionString))
                {
                    if ((InEvenPool(Event.EventId))&&((db.OfficeDCEvents.FirstOrDefault(e => e.EventThumb == Event.EventThumb)) == null))
                    {
                        //SQL group
                        string TargetGroupName = Event.TargetUserName;
                        var temp_group = db.OfficeDCGroups.FirstOrDefault(g => g.GroupName == TargetGroupName);
                        int TargetGroupId = temp_group.id;
                        var Group = db.OfficeDCGroups.FirstOrDefault(g => g.GroupName == TargetGroupName);
                        var select = from ug in db.OfficeDCUserGroups
                                     join u in db.OfficeDCUsers
                                     on ug.UserId equals u.id into group1
                                     where (true)
                                     from g1 in group1.DefaultIfEmpty()
                                     where (ug.GroupId == Group.id)
                                     select new User
                                     {
                                         UserId = ug.UserId,
                                         UserName = g1.UserFIO
                                     };

                        List<User> SQLUsers = new List<User>();
                        foreach (User u in select)
                        {
                            SQLUsers.Add(new User
                            {
                                UserId = u.UserId,
                                UserName = u.UserName
                            });
                        }

                        //AD group
                        
                        DirectoryEntry entry = new DirectoryEntry();
                        foreach (var domain in DomainConfig)
                        {
                            if (Event.Computer.ToLower().Contains(domain.ToString().ToLower()))
                            {
                                entry = new DirectoryEntry("LDAP://" + domain.First["DC"].Value<string>(),
                                    domain.First["Username"].Value<string>(), domain.First["Password"].Value<string>());
                                DomainName = domain.ToString();
                                var temp =
                                    db.Domains.FirstOrDefault(d => d.DomainName.ToLower().Contains(DomainName.ToLower()));
                                Event.DomainId = temp.id;
                            }
                        }
                        DirectorySearcher ds = new DirectorySearcher(entry);

                        ds.Filter = String.Format("(&(cn={0})(objectClass=group))", TargetGroupName);
                        ds.PropertiesToLoad.Add("member");
                        SearchResultCollection sr = ds.FindAll();
                        List<User> ADUsers = new List<User>();
                        foreach (var UserCn in sr[0].Properties["member"])
                        {
                            string cn = UserCn.ToString();
                            cn = cn.Substring(3, cn.IndexOf(",") - 3);
                            var ADUser = db.OfficeDCUsers.FirstOrDefault(u => u.UserFIO == cn);
                            if (ADUser != null)
                            {
                                ADUsers.Add(new User
                                {
                                    UserName = cn,
                                    UserId = ADUser.id
                                });
                            }
                        }

                        //Added users
                        List<User> addedUsers = new List<User>();

                        //Removed users
                        List<User> removedUsers = new List<User>();

                        //Loop all ADUsers
                        foreach (User u in ADUsers)
                        {
                            User toRemove = SQLUsers.Find(us => us.UserName == u.UserName);
                            if (toRemove != null)
                            {
                                SQLUsers.Remove(toRemove);
                            }
                            else //add new users
                            {
                                OfficeDCUserGroups toAdd = new OfficeDCUserGroups
                                {
                                    GroupId = TargetGroupId,
                                    UserId = u.UserId,
                                    ModifiedDate = DateTime.Now
                                };
                                addedUsers.Add(new User
                                {
                                    UserId = u.UserId,
                                    UserName = u.UserName
                                });
                                db.Entry(toAdd).State = EntityState.Added;
                                db.SaveChanges();
                            }

                        }

                        //Loop all remaining SQL users
                        foreach (User u in SQLUsers)
                        {
                            var toRemove = db.OfficeDCUserGroups.FirstOrDefault(ug => ((ug.UserId == u.UserId) && (ug.GroupId == TargetGroupId)));
                            removedUsers.Add(new User
                            {
                                UserId = u.UserId,
                                UserName = u.UserName
                            });
                            db.Entry(toRemove).State = EntityState.Deleted;
                            db.SaveChanges();
                        }

                        if (IsAddEvent(Event.EventId))
                        {
                            Event.GroupMemberId = addedUsers[0].UserId;
                        }
                        else
                        {
                            Event.GroupMemberId = removedUsers[0].UserId;
                        }

                        if (InEventMailNotifPool(Event.EventId))
                        {
                            SendMail(DomainName, Event);
                        }

                        log.Info("Event added to db", this.ToString(), Event);

                        db.Entry(Event).State = EntityState.Added;
                        db.SaveChanges();
                    }
                }
            }

            log.Info("Dicts parsing complited", this.ToString());
        }

        public override void PublishResult()
        {
            SecCore.TaskEnded(id, "Waiting");
        }

        public override void Dispose()
        {
        }

        private string Hash(string Input)
        {
            byte[] lul = Encoding.ASCII.GetBytes(Input);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] Thumb = sha.ComputeHash(lul, 0, lul.Length);
            var sb = new StringBuilder();
            foreach (byte b in Thumb) sb.AppendFormat("{0:x2}", b);

            return BitConverter.ToString(Thumb).Replace("-", "");
        }

        private string GetId(OfficeDCEvents Event)
        {
            return Event.Date.ToString() + Event.SubjectUserSid + Event.SubjectUserName + Event.SubjectDomainName +
                              Event.TargetUserName + Event.TargetDomainName
                              + Event.Computer + Event.EventId + Event.DomainId;
        }

        private bool InEvenPool(int id)
        {
            if (
                (id == 4728) ||
                (id == 4729) ||
                (id == 4732) ||
                (id == 4733) ||
                (id == 4756) ||
                (id == 4757)
            )
            {
                return true;
            }
            return false;
        }

        bool IsAddEvent(int id)
        {
            if (
                (id == 4728) ||
                (id == 4732) ||
                (id == 4756)
            )
            {
                return true;
            }
            return false;
        }

        bool InEventMailNotifPool(int id)
        {
            if (
                (id == 4728) ||
                (id == 4729) ||
                (id == 4732) ||
                (id == 4733) ||
                (id == 4756) ||
                (id == 4757) ||
                (id == 4720)
            )
            {
                return true;
            }
            return false;
        }
    }
}
