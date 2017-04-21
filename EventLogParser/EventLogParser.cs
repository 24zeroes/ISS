using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppPattern;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Production
{
    public class EventLogParser : Application
    {
        [JsonIgnore]
        private JToken EventLogParserConfig;
        [JsonIgnore]
        private JToken EventLogParserDbConfig;
        [JsonIgnore]
        private string cubeConnectionString;


        public override void GetConfiguration()
        {
            SecCore.TaskStarted(id);
            
        }

        public override void InitialiseInputData()
        {

        }

        public override void ProcessData()
        {

        }

        public override void PublishResult()
        {
            SecCore.TaskEnded(id, "Waiting");
        }

        public override void Dispose()
        {
        }
    }
}
