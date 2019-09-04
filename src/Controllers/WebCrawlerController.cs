using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Service.iSwarm;

namespace src.Controllers
{
    [Route("api/[controller]")]
    public class WebCrawlerController : Controller
    {
        private readonly IWebCrawlerService service;

        public WebCrawlerController(IWebCrawlerService service)
        {
            this.service = service;
        }

        [HttpGet]
        public string Get()
        {
            var result = this.service.GetLiquidityAdequacyRequirementsPage();
            return result;
        }

        [HttpGet("getWeb")]
        
        public string GetWeb()
        {
            var result = this.service.GetLiquidityAdequacyRequirementsPageWithCovenants();
            return result;
        }

        [HttpGet("getCovenants")]

        public object GetWebCovenants()
        {
            var list = this.service.GetCovenants();
            return list.Select(x => new
            {
                id = x.CovenantId,
                type = x.CovenantType,
                description = x.CovenantValue,
                state = x.State.ToString()
            });
        }
    }
}
