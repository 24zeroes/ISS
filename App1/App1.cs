using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppPattern;
using Newtonsoft.Json.Linq;
using SecurityProvider;
using Sodium;

namespace Test
{
    public class App1 : Application
    {
        public App1(ref SecurityCore SecCore, List<JToken> Roles)
        {
            var App1KeyPair = PublicKeyBox.GenerateKeyPair();

            KeyPair SecurityResponse = SecCore.RegisterService("App1", Roles, App1KeyPair.PublicKey);

            //byte[] App1Nonce = SecurityResponse.PublicKey;

            //byte[] SecPublicKey = SecurityResponse.PrivateKey;

            //byte[] EncryptedRequest = PublicKeyBox.Create("TelBook", App1Nonce, App1KeyPair.PrivateKey, SecPublicKey);

            JToken App1Config = SecCore.GetProtectedInfo("App1", "TelBook");
        }


        public override void GetConfiguration()
        {
           
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
