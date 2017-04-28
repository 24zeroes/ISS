using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Application_Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataLayer.Application_Models.DC_Parser;
using DataLayer.DB_Models.CubeMonitoring;
using Application = AppPattern.Application;

namespace Production
{
    public partial class DCParser : Application
    {
        private DBCred DCConfig;
        private List<FullDomain> Groups;

        public override void GetConfiguration()
        {
            SecCore.TaskStarted(id);
            DCConfig = SecCore.Configuration.DBCreds.FirstOrDefault(c => c.DB == "Cube");

            Groups = new List<FullDomain>();
            log.Info("Initialisation sucessfull", this.ToString());
        }

        public override void InitialiseInputData()
        {

            foreach (var Domain in SecCore.Configuration.Domains)
            {
                if (Domain.Name.ToLower().Contains(SecCore.Instance.ToLower()))
                    GetDomainGroups(Domain);
        
            }
        }

        public override void ProcessData()
        {

            

            foreach (FullDomain Domain  in Groups)
            {
                Domains domain;

                using (var db = new CubeMonitoring(DCConfig.ConnectionString))
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
