using Microsoft.AspNetCore.Mvc;
using src.Service.Upload;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace src.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        [HttpGet("[action]")]
        public string Ping() => "Pong";

        [HttpGet("[action]")]
        public async Task<string> WriteReadFile([FromServices]IStorage storage)
        {
            var path = await storage.SaveAsync(".tmp", Encoding.UTF8.GetBytes("Pong"), "Ping.txt");
            return await storage.ReadAsync(".tmp", "Ping.txt");
        }
    }
}
