using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SecurityProvider;
using Sodium;
using Test;


namespace ISS
{
    class ISS
    {
        static void Main(string[] args)
        {
            #region CRYPTO

            var SecCore = new SecurityCore(@"InputFiles/", @"OutputFiles/", false);

            //Initial config
            var ISSKeyPair = PublicKeyBox.GenerateKeyPair();

            KeyPair SecurityResponse = SecCore.RegisterService("ISS", new List<string> { "ISS" }, ISSKeyPair.PublicKey);

            byte[] ISSNonce = SecurityResponse.PublicKey;

            byte[] SecPublicKey = SecurityResponse.PrivateKey;

            byte[] EncryptedRequest =
                PublicKeyBox.Create("ISS", ISSNonce, ISSKeyPair.PrivateKey, SecPublicKey);

            JToken ISSConfig = SecCore.GetProtectedInfo("ISS", EncryptedRequest);


            #endregion
            
            #region App1_SCHEDULER

            var App1Interval = ISSConfig["App1"]["IntervalInMilliseconds"];
            var App1Roles = ISSConfig["App1"]["Roles"].ToArray();
            //Testing to schedule app1
            Task perdiodicTask = PeriodicTaskFactory.Start(() =>
            {
                using (var testingApp1 = new App1(ref SecCore, App1Roles)) testingApp1.Run();

            }, intervalInMilliseconds: App1Interval.Value<int>() // fire every two seconds...
               );           // for a total of 10 iterations...
            perdiodicTask.ContinueWith(_ =>
            {
                using (var testingApp1 = new App1()) testingApp1.Run();

            }).Wait();

    
            #endregion

        }


    }
}
