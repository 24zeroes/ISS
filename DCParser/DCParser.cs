using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
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
using DataLayer.DB_Models.CubeMonitoring;

namespace Production
{
    public partial class DCParser : Application
    {
        [JsonIgnore] private JToken DCParserConfig;
        [JsonIgnore] private JToken DCParserDbConfig;
        [JsonIgnore] private string cubeConnectionString;
        private List<FullDomain> Groups;

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
            cubeConnectionString = SecCore.GetProtectedInfo("DCParser", "DB_Cube")["ConnectionString"].Value<string>();
            Groups = new List<FullDomain>();
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

            

            foreach (FullDomain Domain  in Groups)
            {
                Domains domain;

                using (var db = new CubeMonitoring(cubeConnectionString))
                {
                    domain = db.Domains.FirstOrDefault(d => d.DomainName == Domain.DomainName);
                }

                if (domain != null)
                {
                    Domain.DomainId = domain.id;

                    UpdateDomainGroups(Domain);
                    UpdateUsers(Domain);
                    UpdateUserGroups(Domain);
                    UpdateComputers(Domain);

                }
                else
                {
                    log.SemanticError($"Domain {Domain.DomainName} was not found in DB", this.ToString(), domain);    
                }


                
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
