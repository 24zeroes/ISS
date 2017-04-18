using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggingProvider;
using Newtonsoft.Json;
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

        #endregion
    }
}
