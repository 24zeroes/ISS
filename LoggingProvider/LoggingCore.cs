using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DB_Models.Logging;
using Newtonsoft.Json.Linq;
using SecurityProvider;
using Sodium;

namespace LoggingProvider
{
    public class LoggingCore
    {
        private KeyPair LoggerKeyPair;
        readonly SecurityCore SecCore;
        private JToken LoggerConfig;
        public LoggingCore(ref SecurityCore SecCore)
        {
            this.SecCore = SecCore;
            this.LoggerKeyPair = PublicKeyBox.GenerateKeyPair();
            KeyPair SecurityResponse = SecCore.RegisterService("Logger", new List<JToken> { "Logger" }, LoggerKeyPair.PublicKey);

            JToken LoggerConfig = SecCore.GetProtectedInfo("Logger", "Logger");

            var db = new Logger(LoggerConfig.Value<string>());
        }
    }
}
