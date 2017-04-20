using AppPattern;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Test
{
    public class App1 : Application
    {

        [JsonIgnore] private JToken App1Config;
        [JsonIgnore] private JToken App1Db;


        public override void GetConfiguration()
        {

            this.App1Config = SecCore.GetProtectedInfo("DCParser");
            this.App1Db = SecCore.GetProtectedInfo("DB_Cube");
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
