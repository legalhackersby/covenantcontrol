using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace src.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        private const string StorageDirectory = ".storage";

        [HttpGet("[action]")]
        public string Ping() => "Pong";

        [HttpGet("[action]")]
        public async Task<string> WriteReadFile()
        {
            if (!Directory.Exists(StorageDirectory))
            {
                Directory.CreateDirectory(StorageDirectory);
            }

            string filePath = Path.Combine(StorageDirectory, "Ping.txt");

            await System.IO.File.WriteAllTextAsync(filePath, "Pong");

            return await System.IO.File.ReadAllTextAsync(filePath);
        }
    }
}
