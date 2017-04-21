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
    class DisplayScheduler
    {
        public static void Start(ref SecurityCore SecCore, int interval)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<Display>().Build();
            job.JobDataMap["security"] = SecCore;

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger0", "group0")     // идентифицируем триггер с именем и группой
                .StartNow()                            // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    .WithIntervalInSeconds(interval)          // через 1 минуту
                    .RepeatForever())                   // бесконечное повторение
                .Build();                               // создаем триггер
            scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}
