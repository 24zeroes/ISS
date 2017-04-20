﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ISS.Schedulers;
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

            var SecCore = new SecurityCore(@"InputFiles/", @"OutputFiles/", true);

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


            #region DCParser_SCHEDULER

            var DCParserInterval = ISSConfig["DCParser"]["IntervalInMinutes"];
            var DCParserRoles = ISSConfig["DCParser"]["Roles"].ToList();
            //Testing to schedule DCParser

            DCParserScheduler.Start(ref SecCore, ref log, DCParserRoles, DCParserInterval.Value<int>());



            #endregion
        }


    }
}
