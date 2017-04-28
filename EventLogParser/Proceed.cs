using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DB_Models.CubeMonitoring;

namespace Production
{
    public partial class EventLogParser
    {
        private void ProceedGroupAdd(OfficeDCEvents Event)
        {
            using (var db = new CubeMonitoring(cubeConnectionString))
            {
                if ((db.OfficeDCEvents.FirstOrDefault(e => e.EventThumb == Event.EventThumb)) == null)
                {
                    var DbGroup = db.OfficeDCGroups.FirstOrDefault(g => g.GroupName == Event.TargetUserName);

                    if (DbGroup == null)
                    {
                        var newGroup = new OfficeDCGroups
                        {
                            GroupName = Event.TargetUserName,
                            GroupDateModified = DateTime.Now
                        };
                        db.Entry(newGroup).State = EntityState.Added;
                    }

                    db.Entry(Event).State = EntityState.Added;
                    db.SaveChanges();

                    SendMail(DomainName, Event);
                }
            }

        }
        private void ProceedUserAdd(OfficeDCEvents Event)
        {
            using (var db = new CubeMonitoring(cubeConnectionString))
            {
                if ((db.OfficeDCEvents.FirstOrDefault(e => e.EventThumb == Event.EventThumb)) == null)
                {

                }
            }
        }
        
        private void ProceedUserGroupChange(OfficeDCEvents Event)
        {
            using (var db = new CubeMonitoring(cubeConnectionString))
            {
                if ((db.OfficeDCEvents.FirstOrDefault(e => e.EventThumb == Event.EventThumb)) == null)
                {

                }
            }
        }
        
    }
}
