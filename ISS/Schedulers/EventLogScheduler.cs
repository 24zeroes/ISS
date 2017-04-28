using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggingProvider;
using Newtonsoft.Json.Linq;
using Production;
using Quartz;
using Quartz.Impl;
using SecurityProvider;

namespace ISS.Schedulers
{
    class EventLogScheduler
    {
        public static void Start(ref SecurityCore SecCore, ref LoggingCore log)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            var temp = SecCore.Configuration.Applications.FirstOrDefault(a => a.Name == "EventLogParser");
            int interval = temp.IntervalInMinutes;

            IJobDetail job = JobBuilder.Create<EventLogParser>().Build();
            job.JobDataMap["security"] = SecCore;
            job.JobDataMap["logging"] = log;
            job.JobDataMap["id"] = SecCore.RegisterTask("EventLogParser", $"Every {interval} minutes");



            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger3", "group3")     // идентифицируем триггер с именем и группой
                .StartNow()                            // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    .WithIntervalInMinutes(interval)          // через 1 минуту
                    .RepeatForever())                   // бесконечное повторение
                .Build();                               // создаем триггер
            log.Info("EventLogParser configured to schedule", "ISS.Schedulers");
            scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}
