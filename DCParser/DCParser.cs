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
    public partial class DCParser : Application
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

            foreach (var Domain in DCParserConfig)
            {

                GetDomainGroups(Domain);
        
            }
        }

        public override void ProcessData()
        {

            cubeConnectionString = SecCore.GetProtectedInfo("DCParser", "DB_Cube")["ConnectionString"].Value<string>();

            foreach (FullGroups Domain  in Groups)
            {

                UpdateDomainGroups(Domain);

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
