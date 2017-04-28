using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Application_Models;
using DataLayer.DB_Models.CubeMonitoring;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataLayer.Application_Models.EventLog;
using Quartz.Spi;
using Application = AppPattern.Application;

namespace Production
{
    public partial class EventLogParser : Application
    {
        [JsonIgnore]
        private EventConfig EventLogConfig;

        private DBCred DbConfig;

        private List<DataLayer.Application_Models.Domain> DomainConfig;

        private List<EventRecord> EventRecords; 

        private List<Dictionary<string, string>> dicts;
        private string DomainName;
        private int DomainId;

        public override void GetConfiguration()
        {
            SecCore.TaskStarted(id);
            EventLogConfig = SecCore.Configuration.EventConfig;
            DbConfig = SecCore.Configuration.DBCreds.FirstOrDefault(c => c.DB == "Cube");

            DomainConfig = SecCore.Configuration.Domains;
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

                using (var db = new CubeMonitoring(DbConfig.ConnectionString))
                {
                    if (((db.OfficeDCEvents.FirstOrDefault(e => e.EventThumb == Event.EventThumb)) == null))
                    {
                        //add
                        //change
                        //remove
                        //target
                        //locked
                        //unlocked
                        //on

                        var TargetEvent = EventLogConfig.Events.FirstOrDefault(e => e.id == Event.id);

                        if (TargetEvent.target.Contains("ug_"))
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
                                if (domain.Name.ToLower().Contains(GetDomainName(Event.Computer)))
                                {
                                    entry = new DirectoryEntry("LDAP://" + domain.Name, domain.Username, domain.Password);
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

                            if (TargetEvent.target.Contains("add"))
                            {
                                Event.GroupMemberId = addedUsers[0].UserId;
                            }
                            else
                            {
                                Event.GroupMemberId = removedUsers[0].UserId;
                            }
                        }

                        if (TargetEvent.notificate == 1)
                        {
                            
                            SendMail(DomainName, Event, TargetEvent.target.Contains("ug_"));
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

        private string GetDomainName(string fqdn)
        {
            int startIndex = fqdn.IndexOf(".");
            int endIndex = fqdn.LastIndexOf(".");
            return fqdn.Substring(startIndex + 1, endIndex - (startIndex + 1));
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

       
    }
}
