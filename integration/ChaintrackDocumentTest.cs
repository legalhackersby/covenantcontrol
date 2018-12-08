using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace integration
{
    public class ChaintrackDocumentTest
    {
        [Fact]
        public async Task GetDocument()
        {
            var api = "api/document/5c0bc13a11a3e42fb0f53675";
            var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(Settings.Host + api);
            Assert.Contains(@"<mark className={'highlight'}>", result);
            Assert.Contains(@"</mark>", result);
            Assert.Contains(@"<br>", result);
            
        }
    }
}

