using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggingProvider;
using Newtonsoft.Json.Linq;
using SecurityProvider;
using Test;

namespace ISS
{
    class Scheduler
    {
        private JToken Interval;
        private List<JToken> Roles;
        private readonly SecurityCore SecCore;
        private readonly LoggingCore log;

        public Scheduler(string AppName, JToken Config, ref SecurityCore SecCore, ref LoggingCore log)
        {
            Interval = Config[AppName]["IntervalInMilliseconds"];
            Roles = Config[AppName]["Roles"].ToList();

            //var lol = Activator.CreateInstance("DCParser");
            //Task perdiodicTask = PeriodicTaskFactory.Start(() =>
            //{
            //    using (var Application = new DCParser(ref SecCore, ref log, App1Roles))
            //    {
            //        try
            //        {
            //            testingDCParser.Run();
            //            log.Append("App1 started", "INFO", "ISS", testingApp1);
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Exception(ex.Message, "ISS", testingApp1);
            //        }

            //    }

            //}, intervalInMilliseconds: DCParser_Interval.Value<int>() // fire every two seconds...
            //       );           // for a total of 10 iterations...
            //perdiodicTask.ContinueWith(_ =>
            //    {
            //        using (var testingApp1 = new App1(ref SecCore, ref log, App1Roles))
            //        {
            //            try
            //            {
            //                testingApp1.Run();
            //                log.Append("App1 started", "INFO", "ISS", testingApp1);
            //            }
            //            catch (Exception ex)
            //            {
            //                log.Exception(ex.Message, "ISS", testingApp1);
            //            }

            //        }

            //    }).Wait();
        }
    }
}
