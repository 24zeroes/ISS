using System;
using System.Collections.Generic;
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

namespace Production
{
    public class DCParser : Application
    {
        [JsonIgnore] private JToken DCParserConfig;
        [JsonIgnore] private JToken DCParserDbConfig;

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



        }

        public override void PublishResult()
        {

        }

        public override void Dispose()
        {
        }
    }
}
