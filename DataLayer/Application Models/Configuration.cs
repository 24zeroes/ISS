using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Application_Models
{
    public class Application
    {
        public string Name { get; set; }
        public int IntervalInMinutes { get; set; }
    }

    public class Domain
    {
        public string Name { get; set; }
        public string DC { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class DBCred
    {
        public string DB { get; set; }
        public string ConnectionString { get; set; }
    }

    public class Event
    {
        public int id { get; set; }
        public string descr { get; set; }
        public string target { get; set; }
        public int notificate { get; set; }
    }

    public class EmailSettings
    {
        public string From { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int Ssl { get; set; }
        public List<string> To { get; set; }
    }

    public class EventConfig
    {
        public List<Event> Events { get; set; }
        public EmailSettings EmailSettings { get; set; }
    }

    public class IMAPCred
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RootObject
    {
        public List<Application> Applications { get; set; }
        public List<Domain> Domains { get; set; }
        public List<DBCred> DBCreds { get; set; }
        public EventConfig EventConfig { get; set; }
        public List<IMAPCred> IMAPCreds { get; set; }
    }
}
