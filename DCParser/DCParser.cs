using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppPattern;
using LoggingProvider;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecurityProvider;
using Sodium;

namespace DCParser
{
    public class DCParser : Application
    {
        [JsonIgnore]
        private JToken DCParserConfig;
        [JsonIgnore]
        private JToken DCParserDbConfig;

        public DCParser(ref SecurityCore SecCore, ref LoggingCore log, List<JToken> Roles)
        {
            this.log = log;
            this.SecCore = SecCore;
            this.AppKeyPair = PublicKeyBox.GenerateKeyPair();
            try
            {
                KeyPair SecurityResponse = SecCore.RegisterService("DCParser", Roles, AppKeyPair.PublicKey);
                log.Info("Registered sucessfull", this.ToString());
            }
            catch (Exception ex)
            {
                log.Exception(ex.Message, this.ToString(), this);
            }
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
