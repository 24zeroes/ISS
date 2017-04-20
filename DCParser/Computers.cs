using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.DirectoryServices;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Application_Models.DC_Parser;
using DataLayer.DB_Models.CubeMonitoring;

namespace Production
{
    public partial class DCParser
    {
        private void UpdateComputers(FullDomain Domain)
        {
            log.Info($"Domain {Domain.DomainName} computers update started", ToString());
            Domain.DirectorySearcher.Filter = "(&(objectClass=computer))";
            Domain.SearchResult = Domain.DirectorySearcher.FindAll();
            foreach (SearchResult computer in Domain.SearchResult)
            {
                using (var db = new CubeMonitoring(cubeConnectionString))
                {
                    int indexCN = computer.Path.IndexOf("CN=");
                    int indexComa = computer.Path.IndexOf(",");
                    string temp = computer.Path.Substring(indexCN + 3,
                    indexComa - (indexCN + 3));
                    string ComputerName = temp;
                    string ComputerIP = GetIPFromName(ComputerName);

                    var CurrentComputer = db.OfficeDCComputers.FirstOrDefault(g => g.ComputerName == ComputerName);

                    if (CurrentComputer != null)
                    {
                        if ((CurrentComputer.ComputerIP != ComputerIP) && (ComputerIP != null))
                        {
                            CurrentComputer.ComputerIP = ComputerIP;
                            CurrentComputer.ComputerDateModified = DateTime.Now;
                            db.Entry(CurrentComputer).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    else
                    {

                        var Computer = new OfficeDCComputers()
                        {
                            ComputerName = ComputerName,
                            ComputerIP = ComputerIP,
                            DomainId = Domain.DomainId,
                            ComputerDateModified = DateTime.Now
                        };
                        db.Entry(Computer).State = EntityState.Added;
                        db.SaveChanges();

                    }
                }
            }
            log.Info($"Domain {Domain.DomainName} computer update complited", ToString());
        }

        private string GetIPFromName(string name)
        {
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(name, 1);
                if (reply.Status != IPStatus.TimedOut)
                    return reply.Address.ToString();
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            return null;
        }
    }
}
