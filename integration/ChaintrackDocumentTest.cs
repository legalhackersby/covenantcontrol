using System.Net.Http;
using System.Threading.Tasks;
using src.Models;
using Xunit;

namespace integration
{
    public class ChaintrackDocumentTest
    {
        [Fact]
        public async Task GetDocument()
        {
            var api = "api/document/5c0c0fc9e038c1681c74a854";
            var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(Settings.Host + api);
            Assert.Contains(@"<mark covenantId=", result);
            Assert.Contains(@"</mark>", result);
            Assert.Contains(@"<br>", result);            
        }

        [Fact]
        public async Task GetCovenantsDocument()
        {
            var api = "api/document/5c0c0fc9e038c1681c74a854/covenants";
            var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(Settings.Host + api);
            //TODO: COMPARE RIGHT
            Assert.Contains(nameof(CovenantSearchResult.StartIndex).ToUpperInvariant(), result.ToUpperInvariant());    
            Assert.Contains(nameof(CovenantSearchResult.EndIndex).ToUpperInvariant(), result.ToUpperInvariant());   
            Assert.Contains(nameof(CovenantSearchResult.CovenantValue).ToUpperInvariant(), result.ToUpperInvariant());  
            Assert.Contains(nameof(CovenantSearchResult.CovenantId).ToUpperInvariant(), result.ToUpperInvariant());    
        }        

        
    }
}

