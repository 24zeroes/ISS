using System;
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
        [JsonIgnore]
        private KeyPair LoggerKeyPair;
        [JsonIgnore]
        readonly SecurityCore SecCore;
        [JsonIgnore]
        private string dbConnectionString;
        [JsonIgnore]
        private JToken LoggerConfig;

        public LoggingCore(ref SecurityCore SecCore)
        {
            this.SecCore = SecCore;
            this.LoggerKeyPair = PublicKeyBox.GenerateKeyPair();
            KeyPair SecurityResponse = SecCore.RegisterService("Logger", new List<JToken> { "Logger" }, LoggerKeyPair.PublicKey);

            LoggerConfig = SecCore.GetProtectedInfo("Logger", "DB_Logger");

            dbConnectionString = LoggerConfig["ConnectionString"].Value<string>();
        }

        public void Info(string message)
        {

            var record = new CommonLogs
            {
                date = DateTime.Now,
                message = message,
                category = "INFO"
            };

            using (var db = new Logger(dbConnectionString))
            {
                db.Entry(record).State = EntityState.Added;
                db.SaveChanges();
            }
            
        }

        public void Info(string message, string application)
        {
            var record = new CommonLogs
            {
                date = DateTime.Now,
                message = message,
                application = application,
                category = "INFO"
            };

            using (var db = new Logger(dbConnectionString))
            {
                db.Entry(record).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        public void Info(string message, object context)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new MyContractResolver() };
            var json = $"\"{context.ToString()}\" : {JsonConvert.SerializeObject(context, settings)}";
            json = "{" + json + "}";

            var record = new CommonLogs
            {
                date = DateTime.Now,
                message = message,
                category = "INFO",
                context = json
            };

            using (var db = new Logger(dbConnectionString))
            {
                db.Entry(record).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        public void Info(string message, string application, object context)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new MyContractResolver() };
            var json = $"\"{context.ToString()}\" : {JsonConvert.SerializeObject(context, settings)}";
            json = "{" + json + "}";

            var record = new CommonLogs
            {
                date = DateTime.Now,
                message = message,
                category = "INFO",
                application = application,
                context = json
            };

            using (var db = new Logger(dbConnectionString))
            {
                db.Entry(record).State = EntityState.Added;
                db.SaveChanges();
            }
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

            using (var db = new Logger(dbConnectionString))
            {
                db.Entry(record).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        public void Exception(string message, string application, object context)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new MyContractResolver() };
            var json = $"\"{context.ToString()}\" : {JsonConvert.SerializeObject(context, settings)}";
            json = "{" + json + "}";

            var record = new CommonLogs
            {
                date = DateTime.Now,
                message = message,
                category = "EXCEPTION",
                application = application,
                context = json
            };

            using (var db = new Logger(dbConnectionString))
            {
                db.Entry(record).State = EntityState.Added;
                db.SaveChanges();
            }
        }

        public void SemanticError(string message, string application, object context)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new MyContractResolver() };
            var json = $"\"{context.ToString()}\" : {JsonConvert.SerializeObject(context, settings)}";
            json = "{" + json + "}";

            var record = new CommonLogs
            {
                date = DateTime.Now,
                message = message,
                category = "SEMANTIC ERROR",
                application = application,
                context = json
            };

            using (var db = new Logger(dbConnectionString))
            {
                db.Entry(record).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
