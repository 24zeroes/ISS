using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DataLayer.DB_Models.CubeMonitoring;
using DataLayer.Application_Models.EventLog;

namespace Production
{
    partial class EventLogParser
    {
        private void GetEventRecords()
        {
            dicts = new List<Dictionary<string, string>>();

            using (
                var reader = new EventLogReader(@"%SystemRoot%\System32\Winevt\Logs\ForwardedEvents.evtx",
                    PathType.FilePath))
            {

                EventRecord record;
                while ((record = reader.ReadEvent()) != null)
                {
                    Dictionary<string, string> task = new Dictionary<string, string>();
                    task.Add("Log name", record.LogName);
                    task.Add("Date", record.TimeCreated.ToString());
                    task.Add("Computer", record.MachineName);
                    task.Add("EventId", record.Id.ToString());
                    var xml = record.ToXml();
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(record.ToXml());
                    XmlElement xRoot = xDoc.DocumentElement;

                    foreach (XmlNode xnode in xRoot)
                    {
                        // получаем атрибут name
                        if ((xnode.Name == "EventData"))
                        {
                            foreach (XmlNode childnode in xnode.ChildNodes)
                            {
                                if (childnode.Attributes.Count != 0)
                                    task.Add(childnode.Attributes[0].InnerText, childnode.InnerText);
                            }
                        }
                        if ((xnode.Name == "RenderingInfo"))
                        {
                            foreach (XmlNode childnode in xnode.ChildNodes)
                            {
                                if (childnode.Name == "Task")
                                    task.Add("Task", childnode.InnerText);
                            }
                        }

                    }

                    dicts.Add(task);
                    

                }
            }
        
        }

        private OfficeDCEvents GetEventFromDict(Dictionary<string, string> task)
        {
            var Event = new OfficeDCEvents();
            Event.LogName = task["Log name"];
            Event.Date = DateTime.Parse(task["Date"]);
            Event.Computer = task["Computer"];

            if (task.Keys.Contains("Task"))
                Event.Task = task["Task"];

            if (task.Keys.Contains("IpAddress"))
                Event.IpAddress = task["IpAddress"];

            if (task.Keys.Contains("IpPort"))
                Event.IpPort = task["IpPort"];

            if (task.Keys.Contains("SubjectUserSid"))
                Event.SubjectUserSid = task["SubjectUserSid"];

            if (task.Keys.Contains("SubjectUserName"))
                Event.SubjectUserName = task["SubjectUserName"];

            if (task.Keys.Contains("SubjectDomainName"))
                Event.SubjectDomainName = task["SubjectDomainName"];

            if (task.Keys.Contains("TargetUserSid"))
                Event.TargetUserSid = task["TargetUserSid"];

            if (task.Keys.Contains("TargetUserName"))
                Event.TargetUserName = task["TargetUserName"];

            if (task.Keys.Contains("TargetDomainName"))
                Event.TargetDomainName = task["TargetDomainName"];

            if (task.Keys.Contains("Workstation"))
                Event.Workstation = task["Workstation"];

            if (task.Keys.Contains("PackageName"))
                Event.PackageName = task["PackageName"];

            if (task.Keys.Contains("EventId"))
            {
                int EventId = Int32.Parse(task["EventId"]);
                Event.EventId = EventId;
                if (InEvenPool(Event.EventId))
                    Event.EventName = EventLogParserConfig[EventId.ToString()].ToString();
            }

            if (task.Keys.Contains("DnsHostName"))
                Event.DnsHostName = task["DnsHostName"];

            Event.EventThumb = Hash(GetId(Event));

            return Event;
        }
    }
}
