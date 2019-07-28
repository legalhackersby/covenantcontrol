using src.Service.iSwarm;
using Xunit;

namespace integration
{
    public class WebCrawlerTest
    {
        private WebCrawlerService webCrawlerService = new WebCrawlerService();

        [Fact]
        public void Test()
        {
            this.webCrawlerService.HandleData();
        }
    }
}
