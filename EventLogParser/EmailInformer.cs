using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DB_Models.CubeMonitoring;

namespace Production
{
    partial class EventLogParser
    {
        void SendMail(string DomainName, OfficeDCEvents e)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(EventLogParserEmail["From"].ToString());
            SmtpClient smtp = new SmtpClient();
            smtp.Port = Int32.Parse(EventLogParserEmail["Port"].ToString());
            smtp.EnableSsl = (EventLogParserEmail["Ssl"].ToString() == "1")? true : false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(EventLogParserEmail["Login"].ToString(), EventLogParserEmail["Pass"].ToString());
            smtp.Host = EventLogParserEmail["Host"].ToString();

            foreach (string address in EventLogParserEmail["To"])
            {
                mail.To.Add(new MailAddress(address));
            }

            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            char Ecran = '"';
            mail.Body = mail.Body + $"<b>Внесено изменение в AD домена {DomainName}</b>";
            mail.Body = mail.Body + $"<table border={Ecran}1{Ecran} cellpadding={Ecran}0{Ecran} cellspacing={Ecran}0{Ecran} width={Ecran}300{Ecran}>";

            mail.Body = mail.Body + $"<tr align={Ecran}left{Ecran}><td>Дата изменения: </td><b><td>{e.Date}</td></tr></b>";
            mail.Body = mail.Body + $"<tr align={Ecran}left{Ecran}><td>Администратор: </td><b><td>{e.SubjectUserName}</td></tr></b>";
            mail.Body = mail.Body + $"<tr align={Ecran}left{Ecran}><td>Объект изменения: </td><b><td>{e.TargetUserName}</td></tr></b>";
            mail.Body = mail.Body + $"<tr align={Ecran}left{Ecran}><td>Описание изменения: </td><b><td>{e.EventName}</td></tr></b>";

            if (InEvenPool(e.EventId))
            {
                var db = new CubeMonitoring(cubeConnectionString);
                var user = db.OfficeDCUsers.FirstOrDefault(u => u.id == e.GroupMemberId.Value);
                mail.Body = mail.Body +
                            $"<tr align={Ecran}left{Ecran}><td>Пользователь: </td><b><td>{user.UserFIO}</td></tr></b>";
                db.Dispose();
                mail.Body = mail.Body + $"</table>";
                mail.Body = mail.Body + $"<br>";
                mail.Body = mail.Body +
                            $"Более полная информация на странице <a href='http://172.18.5.10/RTS/AceesAlerts'>http://172.18.5.10/RTS/AceesAlerts</a>";
            }
            else
            {
                mail.Body = mail.Body + $"</table>";
                mail.Body = mail.Body + $"<br>";
                mail.Body = mail.Body + $"Более полная информация на странице <a href='http://172.18.5.10/RTS/AllAceesAlerts'>http://172.18.5.10/RTS/AllAceesAlerts</a>";

            }

            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
            mail.Subject = e.EventName;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
