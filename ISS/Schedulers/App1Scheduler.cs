using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LoggingProvider;
using Newtonsoft.Json.Linq;
using Quartz;
using Quartz.Impl;
using SecurityProvider;
using Test;

namespace ISS.Schedulers
{

    class App1Scheduler
    {
        public static void Start(ref SecurityCore SecCore, ref LoggingCore log, List<JToken> Roles, int interval)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<App1>().Build();

            job.JobDataMap["security"] = SecCore;
            job.JobDataMap["logging"] = log;
            job.JobDataMap["roles"] = Roles;

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger1", "group1")     // идентифицируем триггер с именем и группой
                .StartNow()                            // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    .WithIntervalInSeconds(interval)          // через 1 минуту
                    .RepeatForever())                   // бесконечное повторение
                .Build();                               // создаем триггер
            log.Info("App1 configured to schedule", "ISS.Schedulers");

            scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}
