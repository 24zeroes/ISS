using System;
using System.Data.Entity;
using DataLayer.DB_Models.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecurityProvider;


namespace LoggingProvider
{
    public class LoggingCore
    {
        [JsonIgnore]
        readonly SecurityCore SecCore;
        [JsonIgnore]
        private string dbConnectionString;
        [JsonIgnore]
        private JToken LoggerConfig;

        public LoggingCore(ref SecurityCore SecCore)
        {
            this.SecCore = SecCore;

            LoggerConfig = SecCore.GetProtectedInfo("DB_Logger");

            dbConnectionString = LoggerConfig["ConnectionString"].Value<string>();
        }

        public void Info(string message)
        {

            var record = new CommonLogs
            {
                date = DateTime.Now,
                message = message,
                category = "INFO",
                instance = SecCore.Instance
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
                instance = SecCore.Instance,
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
                instance = SecCore.Instance,
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
                instance = SecCore.Instance,
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
                instance = SecCore.Instance,
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
                instance = SecCore.Instance,
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
                instance = SecCore.Instance,
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
