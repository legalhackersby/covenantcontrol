using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        [HttpGet("[action]")]
        public string Ping() => "Pong";

        [HttpGet("[action]")]
        public async Task<string> WriteReadFile()
        {
            await System.IO.File.WriteAllTextAsync(".storage/Ping.txt","Pong");
            return await System.IO.File.ReadAllTextAsync(".storage/Ping.txt");
        }
    }
}
