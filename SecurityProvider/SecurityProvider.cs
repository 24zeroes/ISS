using System;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SecurityProvider
{
    public class SecurityCore
    {
        [JsonIgnore] private string Config;
        public string Instance;

       

        public SecurityCore(string connString, string Key)
        {
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

    }
}