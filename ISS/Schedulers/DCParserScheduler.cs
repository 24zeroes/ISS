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
using Test;

namespace ISS.Schedulers
{
    class DCParserScheduler
    {
        public static void Start(ref SecurityCore SecCore, ref LoggingCore log, List<JToken> Roles, int interval)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<DCParser>().Build();
            job.JobDataMap["security"] = SecCore;
            job.JobDataMap["logging"] = log;
            job.JobDataMap["roles"] = Roles;

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger2", "group2")     // идентифицируем триггер с именем и группой
                .StartNow()                            // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    .WithIntervalInMinutes(interval)          // через 1 минуту
                    .RepeatForever())                   // бесконечное повторение
                .Build();                               // создаем триггер
            log.Info("DCParser configured to schedule", "ISS.Schedulers");
            scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}
