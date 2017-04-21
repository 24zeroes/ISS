using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataLayer.Application_Models.ISS;
namespace SecurityProvider
{
    public class SecurityCore
    {
        [JsonIgnore] private string Config;
        public string Instance;

        public List<Task> Tasks;

        public SecurityCore(string connString, string Key)
        {
            Tasks = new List<Task>();
            Instance = Key;
            var connection = new SqlConnection(connString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {

                throw;
            }
            var get_config = new SqlCommand($"select ConfigValue from config where ConfigKey = '{Key}'", connection);

            string JsonString;

            try
            {
                JsonString = get_config.ExecuteScalar().ToString();
            }
            catch (Exception)
            {
                
                throw;
            }

            Config = JsonString;


        }

        public JToken GetProtectedInfo(string ServiceKey)
        {
            JObject config = JObject.Parse(Config);
            foreach (var item in config)
            {
                if (item.Key == ServiceKey)
                    return item.Value;
            }
            return null;
        }

        public int RegisterTask(string TaskName, string TaskRule)
        {
            Tasks.Add(new Task
            {
                Name = TaskName,
                Rule = TaskRule,
                ExecCount = 0,
                Status = "Registered"
            });
            return (Tasks.Count - 1);
        }

        public void TaskStarted(int id)
        {
            Tasks[id].Status = "Executing";
            
        }

        public void TaskEnded(int id, string Code)
        {
            Tasks[id].Status = Code;
            Tasks[id].ExecCount++;
        }

    }
}