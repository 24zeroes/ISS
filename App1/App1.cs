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

namespace Test
{
    public class App1 : Application
    {

        [JsonIgnore]
        private JToken App1Config;
        [JsonIgnore]
        private JToken App1Db;

        public App1(ref SecurityCore SecCore, ref LoggingCore log, List<JToken> Roles)
        {
            this.log = log;
            this.SecCore = SecCore;
            this.AppKeyPair = PublicKeyBox.GenerateKeyPair();
            KeyPair SecurityResponse = SecCore.RegisterService("App1", Roles, AppKeyPair.PublicKey);
            //byte[] App1Nonce = SecurityResponse.PublicKey;

            //byte[] SecPublicKey = SecurityResponse.PrivateKey;

            //byte[] EncryptedRequest = PublicKeyBox.Create("TelBook", App1Nonce, App1KeyPair.PrivateKey, SecPublicKey);

            
        }


        public override void GetConfiguration()
        {
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
