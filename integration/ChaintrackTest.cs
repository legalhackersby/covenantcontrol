using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace integration
{
    public class ChaintrackTest
    {
        [Fact]
        public async Task Ping()
        {
            var api = "api/health/ping";
            var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(Settings.Host + api);
            Assert.Equal("Pong", result);
        }

        [Fact]
        public async Task File()
        {

            var api = "api/health/writereadfile";
            var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(Settings.Host + api);
            Assert.Equal("Pong", result);
        }
    }
}

