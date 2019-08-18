using System.Collections.Generic;
using Quartz;
using Quartz.Impl;
using src.Jobs;
using src.Service.iSwarm;

namespace src
{
    public class Scheduler
    {
        public static async void Start(IWebCrawlerService service)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var dataMap = new JobDataMap();
            dataMap.Put("WebCrawlerService", service);
            IJobDetail job = JobBuilder.Create<WebCrawlerJob>().UsingJobData(dataMap).Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(5)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
