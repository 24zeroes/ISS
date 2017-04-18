using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppPattern;
using Newtonsoft.Json.Linq;
using SecurityProvider;
using Sodium;
using DataLayer;
using DataLayer.CubeMonitoring;
using LoggingProvider;
using Newtonsoft.Json;
using Quartz;

namespace Test
{
    public class App1 : Application
    {

        [JsonIgnore] private JToken App1Config;
        [JsonIgnore] private JToken App1Db;


        public override void GetConfiguration()
        {
            this.AppKeyPair = PublicKeyBox.GenerateKeyPair();
            this.AppKeyPair = PublicKeyBox.GenerateKeyPair();

            try
            {
                KeyPair SecurityResponse = SecCore.RegisterService("App1", Roles, AppKeyPair.PublicKey);
                log.Info("Registered sucessfull", this.ToString());
            }
            catch (Exception ex)
            {
                log.Exception(ex.Message, this.ToString(), this);
            }

            this.App1Config = SecCore.GetProtectedInfo("App1", "DCParser");
            this.App1Db = SecCore.GetProtectedInfo("App1", "DB_Cube");
        }

        public override void InitialiseInputData()
        {

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
