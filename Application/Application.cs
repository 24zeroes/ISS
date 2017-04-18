using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggingProvider;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using SecurityProvider;
using Sodium;

namespace AppPattern
{
    public abstract class Application : IJob
    {
        #region execution_plan
        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            SecCore = (SecurityCore) dataMap["security"];
            log = (LoggingCore) dataMap["logging"];
            Roles = (List<JToken>) dataMap["roles"];

            GetConfiguration();
            InitialiseInputData();
            ProcessData();
            PublishResult();
        }
        #endregion

        #region methods
        public abstract void GetConfiguration();

        public abstract void InitialiseInputData();

        public abstract void ProcessData();

        public abstract void PublishResult();

        public abstract void Dispose();

        #endregion

        #region properties
        [JsonIgnore]
        protected KeyPair AppKeyPair;
        [JsonIgnore]
        protected SecurityCore SecCore;
        [JsonIgnore]
        protected LoggingCore log;

        [JsonIgnore] protected List<JToken> Roles;


        #endregion
    }
}
