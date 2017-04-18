﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DB_Models.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecurityProvider;
using Sodium;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Serialization;

namespace LoggingProvider
{
    public class LoggingCore
    {
        private KeyPair LoggerKeyPair;
        public SecurityCore SecCore;
        private Logger db;
        private JToken LoggerConfig;

        public LoggingCore(ref SecurityCore SecCore)
        {
            this.SecCore = SecCore;
            this.LoggerKeyPair = PublicKeyBox.GenerateKeyPair();
            KeyPair SecurityResponse = SecCore.RegisterService("Logger", new List<JToken> { "Logger" }, LoggerKeyPair.PublicKey);

            LoggerConfig = SecCore.GetProtectedInfo("Logger", "DB_Logger");

            db = new Logger(LoggerConfig["ConnectionString"].Value<string>());
        }

        public void Append(string message)
        {
            var record = new CommonLogs
            {
                date = DateTime.Now,
                message = message,
                category = "INFO"
            };
            db.Entry(record).State = EntityState.Added;
            db.SaveChanges();
        }

        public void Append(string message, string category, string application, object context)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new MyContractResolver() };
            var json = $"\"{context.ToString()}\" : {JsonConvert.SerializeObject(context, settings)}";
            json = "{" + json + "}";

            var record = new CommonLogs
            {
                date = DateTime.Now,
                message = message,
                category = category,
                application = application,
                context = json
            };
            db.Entry(record).State = EntityState.Added;
            db.SaveChanges();
        }

    }
}