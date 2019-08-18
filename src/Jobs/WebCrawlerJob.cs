using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using src.Service.iSwarm;

namespace src.Jobs
{
    public class WebCrawlerJob : IJob
    {
        

        public WebCrawlerJob()
        {
            
        }

        public async Task Execute(IJobExecutionContext context)
        {
            IWebCrawlerService service = (IWebCrawlerService)context.MergedJobDataMap["WebCrawlerService"];
            await Task.Run(() => service.HandleData()); 
        }
    }
}
