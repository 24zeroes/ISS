using System.Collections.Generic;
using LoggingProvider;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using SecurityProvider;


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
            id = (int) dataMap["id"];
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
        protected SecurityCore SecCore;
        [JsonIgnore]
        protected LoggingCore log;

        protected int id;

        #endregion
    }
}
