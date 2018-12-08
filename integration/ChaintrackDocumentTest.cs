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
            var api = "api/document/acf26d66aa6143cbb138503f91063781";
            var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(Settings.Host + api);
            Assert.Contains(@"<mark className={'highlight'}>", result);
            Assert.Contains(@"</mark>", result);
            Assert.Contains(@"<br>", result);
            
        }
    }
}

