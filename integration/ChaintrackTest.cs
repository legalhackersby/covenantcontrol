using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace integration
{
    public class ChaintrackTest
    {
        [Fact]
        public async Task Test1()
        {
          var local = "http://localhost:5000/";
          var api = "api/health/ping";
          var httpClient = new HttpClient();
          var result = await httpClient.GetStringAsync(local + api);
          Assert.Equal("Pong", result);
        }
    }
}

