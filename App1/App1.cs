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

namespace Test
{
    public class App1 : Application
    {
        
        
        private JToken App1Config;

        public App1(ref SecurityCore SecCore, List<JToken> Roles)
        {
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
        }

        public override void InitialiseInputData()
        {
            using (var db = new CubeMonitoring())
            {
                var item = db.OfficeDCUsers.FirstOrDefault(u => u.UserFIO.Contains("Батейкин"));
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
