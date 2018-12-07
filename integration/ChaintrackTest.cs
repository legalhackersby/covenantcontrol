using System;
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
          var local = "http://localhost:56248/";
          var api = "api/health/ping";
          var httpClient = new HttpClient();
          var result = await httpClient.GetStringAsync(local + api);
          Assert.Equal("Pong", result);
        }

        [Fact]
        public async Task File()
        {
          var local = "http://localhost:56248/";
          var api = "api/health/writereadfile";
          var httpClient = new HttpClient();
          var result = await httpClient.GetStringAsync(local + api);
          Assert.Equal("Pong", result);
        }        
    }
}

