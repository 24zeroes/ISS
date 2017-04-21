using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ISS.Schedulers;
using LoggingProvider;
using Newtonsoft.Json.Linq;
using SecurityProvider;
using Test;


namespace ISS
{
    class ISS
    {
        static void Main(string[] args)
        {
           

            Console.Write("Login: ");
            string Username = Console.ReadLine();
            Console.Write("Password: ");
            string Password = Console.ReadLine();
            Console.Write("Key: ");
            string Key = Console.ReadLine();
            Console.Clear();

            
            
            


            
            string ConnectionString =   
                $"user id={Username};" +
                   $"password={Password};server=172.18.27.75;" +
                   "Trusted_Connection=no;" +
                   "database=ISS; " +
                   "connection timeout=30";

            #region CRYPTO

            SecurityCore SecCore;
            try
            {
                SecCore = new SecurityCore(ConnectionString, Key);
            }
            catch (Exception)
            {
                Console.WriteLine("GTFO");
                throw;
            }

            JToken ISSConfig = SecCore.GetProtectedInfo("ISS");

            #endregion
            DisplayScheduler.Start(ref SecCore, 1);
            #region Logger

            var log = new LoggingCore(ref SecCore);

            //log.Append("test");
            log.Info("ISS successfully started");
            log.Append("Security service successfully started", "INFO", "ISS", SecCore);
            log.Append("Logging service successfully started", "INFO", "ISS", log);

            #endregion

            

            #region DCParser_SCHEDULER

            var DCParserInterval = ISSConfig["DCParser"]["IntervalInMinutes"];
            var DCParserRoles = ISSConfig["DCParser"]["Roles"].ToList();
            //Testing to schedule DCParser

            DCParserScheduler.Start(ref SecCore, ref log, DCParserRoles, DCParserInterval.Value<int>());



            #endregion
        }


    }
}
