using MongoDB.Driver;
using src.Repository;
using src.Service.iSwarm;
using Xunit;

namespace integration
{
    public class WebCrawlerTest
    {
        private IWebCrawlerService webCrawlerService;

        public WebCrawlerTest()
        {
            var client = new MongoClient();
            this.webCrawlerService = new WebCrawlerService(new ChapterMongoRepository(client.GetDatabase("testCovenantControl")));
        }

        [Fact]
        public void Test()
        {
            this.webCrawlerService.HandleData();
        }
    }
}
