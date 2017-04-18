using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LoggingProvider;
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

            KeyPair SecurityResponse = SecCore.RegisterService("ISS", new List<JToken> {"ISS"}, ISSKeyPair.PublicKey);

            //byte[] ISSNonce = SecurityResponse.PublicKey;

            //byte[] SecPublicKey = SecurityResponse.PrivateKey;

            //byte[] EncryptedRequest = PublicKeyBox.Create("ISS", ISSNonce, ISSKeyPair.PrivateKey, SecPublicKey);

            JToken ISSConfig = SecCore.GetProtectedInfo("ISS", "ISS");


            #endregion

            #region Logger

            var log = new LoggingCore(ref SecCore);

            //log.Append("test");
            log.Info("ISS successfully started");
            log.Append("Security service successfully started", "INFO", "ISS", SecCore);
            log.Append("Logging service successfully started", "INFO", "ISS", log);

            #endregion

            #region App1_SCHEDULER

            var App1Interval = ISSConfig["App1"]["IntervalInMilliseconds"];
            var App1Roles = ISSConfig["App1"]["Roles"].ToList();

            using (var testingApp1 = new App1(ref SecCore, ref log, App1Roles))
            {
                try
                {
                    testingApp1.Run();
                    log.Append("App1 started", "INFO", "ISS", testingApp1);
                }
                catch (Exception ex)
                {
                    log.Exception(ex.Message, "ISS", testingApp1);
                }
                
            }
            //Testing to schedule app1
            Task perdiodicTask = PeriodicTaskFactory.Start(() =>
            {
                using (var testingApp1 = new App1(ref SecCore, ref log, App1Roles))
                {
                    try
                    {
                        testingApp1.Run();
                        log.Append("App1 started", "INFO", "ISS", testingApp1);
                    }
                    catch (Exception ex)
                    {
                        log.Exception(ex.Message, "ISS", testingApp1);
                    }

                }

            }, intervalInMilliseconds: App1Interval.Value<int>() // fire every two seconds...
               );           // for a total of 10 iterations...
            perdiodicTask.ContinueWith(_ =>
            {
                using (var testingApp1 = new App1(ref SecCore, ref log, App1Roles))
                {
                    try
                    {
                        testingApp1.Run();
                        log.Append("App1 started", "INFO", "ISS", testingApp1);
                    }
                    catch (Exception ex)
                    {
                        log.Exception(ex.Message, "ISS", testingApp1);
                    }

                }

            }).Wait();

    
            #endregion

        }


    }
}
