using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppPattern;
using LoggingProvider;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecurityProvider;
using Sodium;
using DataLayer.Application_Models.DC_Parser;
using DataLayer.CubeMonitoring;

namespace Production
{
    public class DCParser : Application
    {
        [JsonIgnore] private JToken DCParserConfig;
        [JsonIgnore] private JToken DCParserDbConfig;
        [JsonIgnore] private string cubeConnectionString;
        private List<FullGroups> Groups;

        public override void GetConfiguration()
        {
            this.AppKeyPair = PublicKeyBox.GenerateKeyPair();
            try
            {
                KeyPair SecurityResponse = SecCore.RegisterService("DCParser", Roles, AppKeyPair.PublicKey);
                log.Info("Registered sucessfull", this.ToString());
            }
            catch (Exception ex)
            {
                log.Exception("Registration error. Exception message:" + ex.Message, this.ToString(), this);
                throw;
            }

            DCParserConfig = SecCore.GetProtectedInfo("DCParser", "DCParser");
            Groups = new List<FullGroups>();
            log.Info("Initialisation sucessfull", this.ToString());
        }

        public override void InitialiseInputData()
        {
            //var Config = JsonConvert.DeserializeObject<DCConfig>(JsonConvert.SerializeObject(DCParserConfig));

            foreach (var Domain in DCParserConfig)
            {
                DirectoryEntry entry;
        
                
                entry = new DirectoryEntry("LDAP://" + Domain.First["DC"].Value<string>(), Domain.First["Username"].Value<string>(), Domain.First["Password"].Value<string>());
                

                DirectorySearcher ds = new DirectorySearcher(entry);

                ds.Filter = "(&(objectClass=group))";
                try
                {
                    Groups.Add(new FullGroups
                    {
                        DomainName = Domain.Path.Replace("DCParser.",""),
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
        }

        public override void ProcessData()
        {

            cubeConnectionString = SecCore.GetProtectedInfo("DCParser", "DB_Cube")["ConnectionString"].Value<string>();

            foreach (FullGroups Domain  in Groups)
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

                            var Group = new OfficeDCGroups
                            {
                                GroupName = Name,
                                GroupDescription = Description,
                                GroupDateModified = DateTime.Now,
                                GroupPath = Path
                            };
                            db.Entry(Group).State = EntityState.Added;
                            db.SaveChanges();

                        }
                    }
                    


                }

                log.Info($"Domain {Domain.DomainName} group update complited", ToString());
            }

        }

        public override void PublishResult()
        {

        }

        public override void Dispose()
        {
        }
    }
}
