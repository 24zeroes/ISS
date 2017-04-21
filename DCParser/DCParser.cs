using System;
using System.Collections.Generic;
using System.Linq;
using AppPattern;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            SecCore.TaskStarted(id);

            DCParserConfig = SecCore.GetProtectedInfo("DCParser");
            cubeConnectionString = SecCore.GetProtectedInfo("DB_Cube")["ConnectionString"].Value<string>();
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
            SecCore.TaskEnded(id, "Waiting");
        }

        public override void Dispose()
        {
        }


    }
}
